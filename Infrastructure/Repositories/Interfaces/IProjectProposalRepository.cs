using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Repositories.Interfaces
{
    internal interface IProjectProposalRepository
    {
        Task<List<ProjectProposal>> GetAllAsync();
        Task<ProjectProposal> GetByIdAsync(Guid id);
        Task CreateAsync(ProjectProposal projectProposal);
        Task UpdateAsync(ProjectProposal projectProposal);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
