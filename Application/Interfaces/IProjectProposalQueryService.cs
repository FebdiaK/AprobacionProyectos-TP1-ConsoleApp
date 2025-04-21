using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces
{
    public interface IProjectProposalQueryService
    {
        Task<ProjectProposal?> GetProjectProposalByIdAsync(Guid proposalId);
        Task<ProjectProposal?> GetProjectProposalFullWithId(Guid proposalId);
        Task<List<ProjectProposal>> GetAllProjectProposalsAsync();
        Task<List<ProjectApprovalStep>> GetApprovalStepsByProposalIdAsync(Guid proposalId);
        Task<User?> GetApproverUserByStepIdAsync(int approverUserId);

    }
}
