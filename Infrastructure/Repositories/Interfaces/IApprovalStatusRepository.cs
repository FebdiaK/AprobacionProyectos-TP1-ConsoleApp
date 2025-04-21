using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Repositories.Interfaces
{
    internal interface IApprovalStatusRepository
    {
        Task<List<ApprovalStatus>> GetAllAsync();

        Task<ApprovalStatus> GetByIdAsync(int id);
    }


}
