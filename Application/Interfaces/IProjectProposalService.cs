using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Application.Interfaces
{
    public interface IProjectProposalService 
    {
        Task<Guid> CreateProjectProposalAsync(ProjectProposal proposal);
        Task<ProjectProposal?> GetProjectProposalByIdAsync(Guid id);
        Task<List<ProjectProposal>> GetAllProjectProposalsAsync();
        Task<bool> ApproveStepAsync(long stepId, int userId, bool approve, string? observations = null);
        Task<List<ProjectApprovalStep>> GetApprovalStepsByProposalIdAsync(Guid proposalId);
        Task<ProjectProposal?> GetProjectProposalFullWithId(Guid id);
        Task<User> GetApproverUserByStepIdAsync(int approverUserId);  
    }
}
