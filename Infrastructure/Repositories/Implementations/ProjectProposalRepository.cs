using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Infrastructure.Repositories.Implementations
{
    internal class ProjectProposalRepository : IProjectProposalRepository
    {
        Task IProjectProposalRepository.CreateAsync(ProjectProposal projectProposal)
        {
            throw new NotImplementedException();
        }

        Task IProjectProposalRepository.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<List<ProjectProposal>> IProjectProposalRepository.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<ProjectProposal> IProjectProposalRepository.GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        Task IProjectProposalRepository.SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        Task IProjectProposalRepository.UpdateAsync(ProjectProposal projectProposal)
        {
            throw new NotImplementedException();
        }
    }
}
