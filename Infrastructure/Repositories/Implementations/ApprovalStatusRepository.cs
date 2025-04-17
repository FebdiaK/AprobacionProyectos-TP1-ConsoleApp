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
    internal class ApprovalStatusRepository : IApprovalStatusRepository
    {
        private readonly AppDbContext _context;
        public ApprovalStatusRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ApprovalStatus>> GetAllAsync()
        {
            return await _context.ApprovalStatuses.ToListAsync();
        }
    }
}
