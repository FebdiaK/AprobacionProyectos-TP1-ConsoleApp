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
    internal class ProjectApprovalStepRepository : IProjectApprovalStepRepository
    {
        private readonly AppDbContext _context;

        public ProjectApprovalStepRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(ProjectApprovalStep step)
        {
            await _context.ProjectApprovalSteps.AddAsync(step);
        }

        public async Task<ProjectApprovalStep?> GetByIdAsync(long stepId)
        {
            return await _context.ProjectApprovalSteps.FindAsync(stepId);
        }

        public async Task<List<ProjectApprovalStep>> GetStepsByProposalIdAsync(Guid proposalId)
        {
            return await _context.ProjectApprovalSteps
                .Where(s => s.ProjectProposalId == proposalId)
                //.OrderBy(s => s.StepOrder)//.Include(s => s.ApproverRole).ThenInclude(r => r.ApprovalRules).ThenInclude(r => r.StepOrder)
                .ToListAsync();
        }
    }

}
