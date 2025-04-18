using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Presentation.Helpers
{
    public static class ProposalSummaryPrinter
    {
        public static void PrintData(ProjectProposal proposal)
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

        public static void PrintOnlyData(ProjectProposal proposal)
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
        }

        public static void PrintOnlyFlow(ProjectProposal proposal)
        {
            Console.WriteLine("\n Flujo de Aprobación:");
            foreach (var step in proposal.ApprovalSteps.OrderBy(s => s.StepOrder))
            {
                Console.WriteLine($" - Paso #{step.StepOrder} | Rol: {step.ApproverRole.Name}");
            }
        }

        public static void PrintOnlyCurrentFlow(ProjectProposal proposal)
        { 
            foreach (var step in proposal.ApprovalSteps.OrderBy(s => s.StepOrder))
            {
                var estado = step.Status.Name;
                var fecha = step.DecisionDate?.ToString("dd/MM/yyyy HH:mm") ?? "Pendiente";
                var observacion = string.IsNullOrWhiteSpace(step.Observations) ? "-" : step.Observations;

                Console.WriteLine($" - Paso #{step.StepOrder} | Rol necesario: {step.ApproverRole.Name} | Estado: {estado} | Fecha: {fecha} | Obs: {observacion}");
            }
        }
    }

}
