using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;

namespace AprobacionProyectos.Application.Services
{
    internal class ApprovalStatusService : IApprovalStatusService
    {
        private readonly IApprovalStatusRepository _aprrovalStatusRepository;
        public ApprovalStatusService(IApprovalStatusRepository aprrovalStatusRepository)
        {
            _aprrovalStatusRepository = aprrovalStatusRepository;
        }
        public async Task<ApprovalStatus> GetApprovalStatusByIdAsync(int id)
        {
            return await _aprrovalStatusRepository.GetByIdAsync(id);
        }
    }
}
