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
        private readonly IProjectProposalService _projectProposalService;
        private readonly IApprovalStatusService _approvalStatusService;

        public ProposalSummaryPrinter(IProjectProposalService projectProposalService, IApprovalStatusService approvalStatusService)
        {
            _projectProposalService = projectProposalService;
            _approvalStatusService = approvalStatusService;
        }
        public void PrintData(ProjectProposal proposal)
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
            Console.WriteLine($" Fecha de creación: {proposal.CreatedAt.ToString("dd/MM/yyyy HH:mm")}");
            Console.WriteLine($" Creado por: {proposal.CreatedBy.Name}");


            Console.WriteLine("\n Flujo de Aprobación:");
            foreach (var step in proposal.ApprovalSteps.OrderBy(s => s.StepOrder))
            {
                Console.WriteLine($" - Paso #{step.StepOrder} | Rol: {step.ApproverRole.Name}");
            }

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
            Console.WriteLine("\n Flujo de Aprobación:");
            foreach (var step in proposal.ApprovalSteps.OrderBy(s => s.StepOrder))
            {
                Console.WriteLine($" - Paso #{step.StepOrder} | Rol: {step.ApproverRole.Name}");
            }
        }

        public async Task PrintOnlyCurrentFlow(ProjectProposal proposal)
        {
            Console.WriteLine("\n Pasos actuales de Aprobación:");

            var steps = await _projectProposalService.GetApprovalStepsByProposalIdAsync(proposal.Id);

            foreach (var step in steps.OrderBy(s => s.StepOrder))
            //foreach (var step in proposal.ApprovalSteps.OrderBy(s => s.StepOrder))
            {
                var estado = await _approvalStatusService.GetApprovalStatusByIdAsync(step.StatusId);
                
                if (estado.Name == null)
                {
                    Console.WriteLine("error al obtener el nombre del estado");
                }

                var estadoActual = estado.Name;
                if (estado.Id == 2)
                {
                    //estadoActual = estado.Name + " por " + step.ApproverUser.Name; 
                    if (step.ApproverUser != null)
                    {
                        estadoActual = estado.Name + " por " + step.ApproverUser.Name;
                    }
                    else
                    {
                        estadoActual = estado.Name + " (sin usuario)";
                    }
                }
                if (estado.Id == 3)
                {
                    //estadoActual = estado.Name + " por " + step.ApproverUser.Name;
                    if (step.ApproverUser != null)
                    {
                        estadoActual = estado.Name + " por " + step.ApproverUser.Name;
                    }
                    else
                    {
                        estadoActual = estado.Name + " (sin usuario)";
                    }
                }

                var fecha = step.DecisionDate?.ToString("dd/MM/yyyy HH:mm") ?? "Pendiente";
                var observacion = string.IsNullOrWhiteSpace(step.Observations) ? "-" : step.Observations;

                Console.WriteLine($" - Paso #{step.StepOrder} | Rol necesario: {step.ApproverRole.Name} | Estado: {estadoActual}   | Fecha: {fecha} | Obs: {observacion}");
            }
            await Task.CompletedTask; //return

        }
    }

}
