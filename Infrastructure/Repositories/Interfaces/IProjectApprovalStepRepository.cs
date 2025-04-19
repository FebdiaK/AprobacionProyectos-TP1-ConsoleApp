using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Repositories.Interfaces
{
    internal interface IProjectApprovalStepRepository
    {
        Task CreateAsync(ProjectApprovalStep step);
        Task<ProjectApprovalStep?> GetByIdAsync(long stepId);
        Task<List<ProjectApprovalStep>> GetStepsByProposalIdAsync(Guid proposalId);
    }
}
