using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Presentation.Helpers
{
    public class ProposalSummaryPrinter
    {
        private readonly IProjectProposalQueryService _projectProposalQueryService;
        private readonly IApprovalStatusService _approvalStatusService;
        public ProposalSummaryPrinter(IApprovalStatusService approvalStatusService, IProjectProposalQueryService projectProposalQueryService)
        {
            _approvalStatusService = approvalStatusService;
            _projectProposalQueryService = projectProposalQueryService;
        }
        public void PrintData(ProjectProposal proposal)
        {
            PrintOnlyData(proposal);
            PrintOnlyFlow(proposal);
        }
        public void PrintOnlyData(ProjectProposal proposal)
        {
            Console.WriteLine($"\n Datos de la propuesta:");
            Console.WriteLine($" ID: {proposal.Id}");
            Console.WriteLine($" Título: {proposal.Title}");
            Console.WriteLine($" Descripción: {proposal.Description}");
            
            Console.WriteLine($" Área: {proposal.Area.Name}");
            Console.WriteLine($" Tipo: {proposal.Type.Name}");
            Console.WriteLine($" Monto estimado: ${proposal.EstimatedAmount}");
            Console.WriteLine($" Duración estimada: {proposal.EstimatedDuration} días");
            Console.WriteLine($" Estado: {proposal.Status.Name}");
            Console.WriteLine($" Fecha de creación: {proposal.CreatedAt:dd/MM/yyyy HH:mm}");
            Console.WriteLine($" Creado por: {proposal.CreatedBy.Name}");
        }
        public void PrintOnlyFlow(ProjectProposal proposal)
        {
            Console.WriteLine("\nFlujo de Aprobación:");
            foreach (var step in proposal.ApprovalSteps.OrderBy(s => s.StepOrder))
            {
                Console.WriteLine($" - Paso #{step.StepOrder} | Rol: {step.ApproverRole.Name}");
            }
        }
        public async Task PrintOnlyCurrentFlow(ProjectProposal proposal)
        {
            Console.WriteLine("\nPasos actuales de Aprobación:");

            var steps = await _projectProposalQueryService.GetApprovalStepsByProposalIdAsync(proposal.Id);

            foreach (var step in steps.OrderBy(s => s.StepOrder))
            {
                var estado = await _approvalStatusService.GetApprovalStatusByIdAsync(step.StatusId);
                
                if (estado.Name == null)
                {
                    Console.WriteLine("Error al obtener el nombre del estado");
                }

                string estadoActual = estado.Name;

                if (estado.Id == 2 || estado.Id == 3)
                {
                    var usuario = step.ApproverUser?.Name ?? "(sin usuario)";
                    estadoActual += $" por {usuario}";
                }

                var fecha = step.DecisionDate?.ToString("dd/MM/yyyy HH:mm") ?? "Pendiente";
                var observacion = string.IsNullOrWhiteSpace(step.Observations) ? "-" : step.Observations;

                Console.WriteLine($" - Paso #{step.StepOrder} | Rol necesario: {step.ApproverRole.Name} | Estado: {estadoActual}   | Fecha: {fecha} | Obs: {observacion}");
            }
            await Task.CompletedTask; //return
        }
    }
}
