using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Infrastructure.Data;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Repositories.Implementations
{
    internal class ApproverRoleRepository : IApproverRoleRepository
    {
        private readonly AppDbContext _context;
        public ApproverRoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Domain.Entities.ApproverRole>> GetAllAsync()
        {
            return await _context.ApproverRoles.ToListAsync();
        }

        public async Task<Domain.Entities.ApproverRole> GetByIdAsync(int id)
        {
            var approverRole = await _context.ApproverRoles
                .Include(r => r.Users)
                .FirstOrDefaultAsync(r => r.Id == id);

            return approverRole == null ? throw new InvalidOperationException($"No se encontró un ApproverRole con el ID {id}.") : approverRole;
        }
    }
}
