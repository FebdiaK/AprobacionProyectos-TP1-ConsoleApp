using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces
{
    internal interface IProjectProposalService
    {
        Task<Guid> CreateProjectProposalAsync(ProjectProposal proposal);
        Task<ProjectProposal?> GetProjectProposalByIdAsync(Guid id);
        Task<List<ProjectProposal>> GetAllProjectProposalsAsync();
        Task<bool> ApproveStepAsync(long stepId, int userId, bool approve, string? observations = null);
        Task<List<ProjectApprovalStep>> GetApprovalStepsByProposalIdAsync(Guid proposalId);
    }
}
