using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Repositories.Interfaces
{
    public interface IProjectProposalRepository
    {
        Task<List<ProjectProposal>> GetAllAsync();
        Task<ProjectProposal?> GetByIdAsync(Guid id);
        Task CreateAsync(ProjectProposal projectProposal);
        Task SaveChangesAsync();

        Task<ProjectProposal?> GetProjectProposalFullWithId(Guid id); 
    }
}
