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
                .ThenInclude(s => s.ApproverRole)
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

        public async Task<ProjectProposal?> GetProjectProposalFullWithId(Guid id)
        {
            return await _context.ProjectProposals
                .Include(p => p.Area)
                .Include(p => p.Type)
                .Include(p => p.Status)
                .Include(p => p.CreatedBy)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.ApproverRole)
                .Include(p => p.ApprovalSteps)
                    .ThenInclude(s => s.Status)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}

