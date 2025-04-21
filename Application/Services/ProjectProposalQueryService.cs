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
    internal class ProjectProposalQueryService : IProjectProposalQueryService
    {
        private readonly IProjectProposalRepository _proposalRepository;
        private readonly IProjectApprovalStepRepository _stepRepository;
        private readonly IUserRepository _userRepository;
        public ProjectProposalQueryService(
            IProjectProposalRepository proposalRepository,
            IProjectApprovalStepRepository stepRepository,
            IUserRepository userRepository)
        {
            _proposalRepository = proposalRepository;
            _stepRepository = stepRepository;
            _userRepository = userRepository;
        }

        public async Task<List<ProjectProposal>> GetAllProjectProposalsAsync()
        {
            return await _proposalRepository.GetAllAsync();
        }

        public async Task<ProjectProposal?> GetProjectProposalByIdAsync(Guid proposalId)
        {
            return await _proposalRepository.GetByIdAsync(proposalId);
        }

        public async Task<ProjectProposal?> GetProjectProposalFullWithId(Guid id)
        {
            return await _proposalRepository.GetProjectProposalFullWithId(id);
        }

        public async Task<List<ProjectApprovalStep>> GetApprovalStepsByProposalIdAsync(Guid proposalId)
        {
            var proposal = await _proposalRepository.GetByIdAsync(proposalId);
            if (proposal == null) return new List<ProjectApprovalStep>();

            return await _stepRepository.GetStepsByProposalIdAsync(proposal.Id);
        }

        public async Task<User?> GetApproverUserByStepIdAsync(int id)
        {
            var step = await _stepRepository.GetByIdAsync(id);
            if (step == null || step.ApproverUserId == null) return null;

            return await _userRepository.GetByIdAsync(step.ApproverUserId.Value);
        }
    }
}
