using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Application.Interfaces
{
    public interface IApprovalWorkflowService
    {
        Task<bool> ApproveStepAsync(long stepId, int userId, bool approve, string? observations = null);

    }
}
