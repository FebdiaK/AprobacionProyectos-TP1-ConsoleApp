using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Presentation.MenuActions;
using AprobacionProyectos.Application.Interfaces;


namespace AprobacionProyectos.Presentation
{
    public class ConsoleMenuService
    {
        private readonly CreateProposalAction _createProposal;
        private readonly ApproveStepAction _approveStep; 
        private readonly ViewProposalStatusAction _viewProposal;

        public ConsoleMenuService(CreateProposalAction createProposal, ApproveStepAction approveStep, ViewProposalStatusAction viewProposal)
        {
            _createProposal = createProposal;
            _approveStep = approveStep;
            _viewProposal = viewProposal;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("===== MENÚ DE APROBACIÓN DE PROYECTOS =====");
                Console.WriteLine("1. Crear nueva propuesta");
                Console.WriteLine("2. Aprobar / Rechazar paso");
                Console.WriteLine("3. Ver estado de una propuesta");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");
                var input = Console.ReadLine();

                Console.Clear();

                switch (input)
                {
                    case "1":
                        await _createProposal.RunAsync();
                        break;
                    case "2":
                        await _approveStep.RunAsync();
                        break;
                    case "3":
                        await _viewProposal.RunAsync();
                        break;
                    case "0":
                        Console.WriteLine(" ¡Hasta luego!");
                        return;
                    default:
                        Console.WriteLine(" Opción no válida.");
                        break;
                }

                Console.WriteLine("\nPresione una tecla para continuar...");
                Console.ReadKey();
                Console.Clear();
            }

        }
    }
}
