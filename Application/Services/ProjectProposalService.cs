using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    internal class ProjectProposalService
    {
        private readonly IProjectProposalRepository _repository;
        private readonly IApprovalRuleRepository _ruleRepository;
        private readonly IProjectApprovalStepRepository _stepRepository;

        public ProjectProposalService(
            IProjectProposalRepository repository,
            IApprovalRuleRepository ruleRepository,
            IProjectApprovalStepRepository stepRepository)
        {
            _repository = repository;
            _ruleRepository = ruleRepository;
            _stepRepository = stepRepository;
        }

        public async Task<Guid> CreateProjectProposalAsync(ProjectProposal proposal)
        {
            proposal.Id = Guid.NewGuid();
            proposal.CreatedAt = DateTime.UtcNow;

            await _repository.CreateAsync(proposal);

            var rules = await _ruleRepository.GetAllAsync();

            var applicableRules = rules
                .Where(r =>
                    (r.AreaId == null || r.AreaId == proposal.AreaId) &&
                    (r.TypeId == null || r.TypeId == proposal.TypeId) &&
                    r.MinAmount <= proposal.EstimatedAmount &&
                    r.MaxAmount >= proposal.EstimatedAmount)
                .OrderBy(r => r.StepOrder)
                .ToList();

            foreach (var rule in applicableRules)
            {
                var step = new ProjectApprovalStep
                {
                    ProjectProposalId = proposal.Id,
                    ApproverRoleId = rule.ApproverRoleId,
                    StatusId = 1, // Pendiente
                    StepOrder = rule.StepOrder,
                    DecisionDate = null,
                    Observations = null
                };

                await _stepRepository.CreateAsync(step);
            }

            await _repository.SaveChangesAsync();
            return proposal.Id;
        }

        public async Task<bool> ApproveStepAsync(long stepId, int userId, bool approve, string? observations = null)
        {
            var step = await _stepRepository.GetByIdAsync(stepId);
            if (step == null || step.UserId != userId)
                return false; // Paso no encontrado o usuario inválido

            var allSteps = await _stepRepository.GetStepsByProposalIdAsync(step.ProjectProposalId);
            var currentStepIndex = allSteps.FindIndex(s => s.Id == stepId);
            if (currentStepIndex == -1 || allSteps.Take(currentStepIndex).Any(s => s.StatusId == 1))
                return false; // No es el paso actual (hay pasos anteriores pendientes)

            step.StatusId = approve ? 2 : 3; // Aprobado = 2, Rechazado = 3
            step.DecisionDate = DateTime.UtcNow;
            step.Observations = observations;

            var proposal = await _repository.GetByIdAsync(step.ProjectProposalId); 
            if (proposal != null)
            {
                if (!approve)
                    proposal.StatusId = 3; // Rechazado
                else if (allSteps.All(s => s.Id == stepId || s.StatusId == 2))
                    proposal.StatusId = 2; // Aprobado
            }

            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectProposal?> GetProposalWithStatusAsync(Guid proposalId)
        {
            return await _repository.GetByIdAsync(proposalId);
        }
    }
}