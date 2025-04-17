using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Data;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Repositories.Implementations
{
    internal class ProjectProposalRepository : IProjectProposalRepository  
    {
        private readonly AppDbContext _context;

        public ProjectProposalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectProposal?> GetByIdAsync(Guid id)
        {
            return await _context.ProjectProposals
                .Include(p => p.Title)
                .Include(p => p.Description)
                .Include(p => p.EstimatedAmount)
                .Include(p => p.EstimatedDuration)
                .Include(p => p.CreatedAt)
                .Include(p => p.Area)
                .Include(p => p.Type)
                .Include(p => p.Status)
                .Include(p => p.CreatedBy)
                .Include(p => p.ApprovalSteps)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProjectProposal>> GetAllAsync()
        {
            return await _context.ProjectProposals
                .Include(p => p.ApprovalSteps)
                .ToListAsync();
        }

        public async Task CreateAsync(ProjectProposal proposal)
        {
            await _context.ProjectProposals.AddAsync(proposal);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

