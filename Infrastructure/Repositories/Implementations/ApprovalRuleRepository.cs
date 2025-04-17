using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Infrastructure.Repositories.Implementations
{
    internal class ApprovalRuleRepository : IApprovalRuleRepository
    {
        public Task<List<ApprovalRule>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
