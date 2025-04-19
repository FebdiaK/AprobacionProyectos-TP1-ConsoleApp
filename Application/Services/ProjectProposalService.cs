using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Data;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    internal class ProjectProposalService : IProjectProposalService
    {
        private readonly IProjectProposalRepository _repository;
        private readonly IApprovalRuleRepository _ruleRepository;
        private readonly IProjectApprovalStepRepository _stepRepository;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IApprovalStatusRepository _approvalStatusRepository; 

        public ProjectProposalService(
            IProjectProposalRepository repository,
            IApprovalRuleRepository ruleRepository,
            IProjectApprovalStepRepository stepRepository,
            IUserRepository userRepository,
            IApprovalStatusRepository approvalStatusRepository, 
            AppDbContext context
            )
        {
            _repository = repository;
            _ruleRepository = ruleRepository;
            _stepRepository = stepRepository;
            _userRepository = userRepository;
            _approvalStatusRepository = approvalStatusRepository;
            _context = context;

        }

        public async Task<Guid> CreateProjectProposalAsync(ProjectProposal proposal)
        {
            //proposal.Id = Guid.NewGuid();
            proposal.CreatedAt = DateTime.UtcNow;

            await _repository.CreateAsync(proposal);

            var rules = await _ruleRepository.GetAllAsync();

            var applicableRules = rules
                .Where(r =>
                    (r.AreaId == null || r.AreaId == proposal.AreaId) &&
                    (r.TypeId == null || r.TypeId == proposal.TypeId) &&
                    r.MinAmount <= proposal.EstimatedAmount &&
                    (r.MaxAmount == 0 || r.MaxAmount >= proposal.EstimatedAmount))
                .OrderBy(r => r.StepOrder)
                .ToList();

            var selectedRules = applicableRules
                .GroupBy(r => r.StepOrder)
                .Select(g => g
                    .OrderByDescending(r => (r.AreaId.HasValue ? 1 : 0) + (r.TypeId.HasValue ? 1 : 0))
                    .First())
                .OrderBy(r => r.StepOrder)
                .ToList();


            foreach (var rule in selectedRules)
            {

                var step = new ProjectApprovalStep
                {
                    ProjectProposalId = proposal.Id,
                    ApproverRoleId = rule.ApproverRoleId,

                    Status = await _approvalStatusRepository.GetByIdAsync(1),
                    StepOrder = rule.StepOrder,
                    DecisionDate = null,
                    Observations = null
                };

                await _stepRepository.CreateAsync(step);
            }

            await _context.SaveChangesAsync(); //me da una excepcion: An error occurred while saving the entity changes. See the inner exception for details.

            return proposal.Id;
        }

        public async Task<List<ProjectApprovalStep>> GetApprovalStepsByProposalIdAsync(Guid proposalId)
        {
            var proposal = await _repository.GetByIdAsync(proposalId);
            if (proposal == null)
                return new List<ProjectApprovalStep>();
            return await _stepRepository.GetStepsByProposalIdAsync(proposal.Id);

        }

        public async Task<bool> ApproveStepAsync(long stepId, int userId, bool approve, string? observations = null)
        {
            var step = await _stepRepository.GetByIdAsync(stepId);

            if (step == null) //tengo que verificar que existe algun paso con ese id
            {   
                Console.WriteLine("Paso no encontrado o usuario invalido1");
                return false; // paso no encontrado o usuario invalido
            }
            var allSteps = await _stepRepository.GetStepsByProposalIdAsync(step.ProjectProposalId);
            var currentStepIndex = allSteps.FindIndex(s => s.Id == stepId);
            if (currentStepIndex == -1 || allSteps.Take(currentStepIndex).Any(s => s.StatusId == 1))
            {
                Console.WriteLine("Paso no encontrado o usuario invalido2");
                return false; // no es el paso actual (hay pasos anteriores pendientes)
            }
            step.StatusId = approve ? 2 : 3; // Aprobado = 2, Rechazado = 3
            var estado = await _approvalStatusRepository.GetByIdAsync(step.StatusId);
            step.StatusId = estado.Id;
            step.DecisionDate = DateTime.UtcNow;
            step.Observations = observations;
            step.ApproverUserId = userId;
            

            var proposal = await _repository.GetByIdAsync(step.ProjectProposalId);
            if (proposal != null)
            {
                if (!approve)
                    proposal.StatusId = 3; // Rechazado
                else if (allSteps.All(s => s.Id == stepId || s.StatusId == 2))
                    proposal.StatusId = 2; // Aprobado
            }

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<List<ProjectProposal>> GetAllProjectProposalsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ProjectProposal?> GetProjectProposalByIdAsync(Guid proposalId)
        {
            return await _repository.GetByIdAsync(proposalId);
        }


        public async Task<ProjectProposal?> GetProjectProposalFullWithId(Guid id)
        {
            var proposal = await _repository.GetProjectProposalFullWithId(id);
            if (proposal == null)
                return null;
            return proposal;
        }

        public async Task<User> GetApproverUserByStepIdAsync(int id)
        {
            var step = await _stepRepository.GetByIdAsync(id);
            if (step == null)
                return null;
            return await _userRepository.GetByIdAsync(step.ApproverUserId ?? 0);
             
        }
    }
}