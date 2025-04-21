using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Presentation.Helpers;

namespace AprobacionProyectos.Presentation.MenuActions
{
    public class ApproveStepAction
    {
        private readonly IProjectProposalQueryService _projectProposalQueryService;
        private readonly IApprovalWorkflowService _approvalWorkflowService;
        private readonly IUserService _userService;
        public ApproveStepAction(IUserService userService, IProjectProposalQueryService projectProposalQueryService, IApprovalWorkflowService approvalWorkflowService)
        {
            _userService = userService;
            _projectProposalQueryService = projectProposalQueryService;
            _approvalWorkflowService = approvalWorkflowService;
        }
        public async Task RunAsync()
        {
            try
            {

                Console.Clear();
                Console.WriteLine("===== APROBAR / RECHAZAR PASO =====");

                var propuestas = await _projectProposalQueryService.GetAllProjectProposalsAsync();
                var propuesta = ProjectSelectionHelper.SelectProposal(propuestas);
                if (propuesta == null)
                    return;

                var paso = propuesta.ApprovalSteps.OrderBy(s => s.StepOrder).FirstOrDefault(s => s.StatusId == 1);
                if (paso == null)
                {
                    Console.WriteLine("No hay paso pendiente.");
                    return;
                }

                var pasosAnterioresPendientes = propuesta.ApprovalSteps
                    .Where(s => s.StepOrder < paso.StepOrder && s.StatusId == 1)
                    .ToList();

                if (pasosAnterioresPendientes.Count > 0)
                {
                    Console.WriteLine("No se puede aprobar este paso porque hay pasos anteriores pendientes.");
                    return;
                }

                var user = await UserValidationHelper.GetValidUserAsync(_userService, paso);
                if (user == null)
                    return;
                
                var decision = ApprovalConfirmationHelper.GetDecision();  
                if (decision == null)
                    return;

                var(aprobado, observaciones) = decision.Value.aprobado ? (true, decision.Value.observaciones) : (false, decision.Value.observaciones);

                var resultado = await _approvalWorkflowService.ApproveStepAsync(paso.Id, user.Id, aprobado, observaciones);

                Console.WriteLine(resultado ? "\n- Decisión tomada con éxito." : " Ocurrió un error al aprobar el paso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ocurrió un error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
                Console.ReadKey();
            }
        }
    }
}
