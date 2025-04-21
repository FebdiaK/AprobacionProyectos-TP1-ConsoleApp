using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Infrastructure.Data;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    internal class ApprovalWorkflowService : IApprovalWorkflowService
    {
        private readonly IProjectApprovalStepRepository _stepRepository;
        private readonly IProjectProposalRepository _proposalRepository;
        private readonly IApprovalStatusRepository _statusRepository;
        private readonly AppDbContext _context;
        public ApprovalWorkflowService(
            IProjectApprovalStepRepository stepRepository,
            IProjectProposalRepository proposalRepository,
            IApprovalStatusRepository statusRepository,
            AppDbContext context)
        {
            _stepRepository = stepRepository;
            _proposalRepository = proposalRepository;
            _statusRepository = statusRepository;
            _context = context;
        }
        public async Task<bool> ApproveStepAsync(long stepId, int userId, bool approve, string? observations = null)
        {
            var step = await _stepRepository.GetByIdAsync(stepId);
            if (step == null) return false;

            var allSteps = await _stepRepository.GetStepsByProposalIdAsync(step.ProjectProposalId);
            var currentStepIndex = allSteps.FindIndex(s => s.Id == stepId);
            if (currentStepIndex == -1 || allSteps.Take(currentStepIndex).Any(s => s.StatusId == 1))
                return false;

            //step.StatusId = approve ? 2 : 3; // Aprobado = 2, Rechazado = 3
            //step.StatusId = (await _statusRepository.GetByIdAsync(step.StatusId)).Id;
            var status = await _statusRepository.GetByIdAsync(approve ? 2 : 3);
            step.StatusId = status.Id;
            step.DecisionDate = DateTime.UtcNow;
            step.Observations = observations;
            step.ApproverUserId = userId;

            var proposal = await _proposalRepository.GetByIdAsync(step.ProjectProposalId);
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
    }
}
