using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Presentation.Helpers;

namespace AprobacionProyectos.Presentation.MenuActions
{
    public class ViewProposalStatusAction
    {
        private readonly IProjectProposalService _projectProposalService;

        public ViewProposalStatusAction(IProjectProposalService projectProposalService)
        {
            _projectProposalService = projectProposalService;
        }

        public async Task RunAsync()
        {
            try
            {
                Console.Clear();

                Console.WriteLine("===== VER ESTADO DE UNA PROPUESTA =====");

                var proyectos = await _projectProposalService.GetAllProjectProposalsAsync();

                if (!proyectos.Any())
                {
                    Console.WriteLine(" No hay proyectos actualmente.");
                    return;
                }

                Console.WriteLine(" Lista de propuestas de Proyectos: ");
                var indexLookup = new Dictionary<int, Guid>();
                int i = 1;

                foreach (var p in proyectos)
                {
                    Console.WriteLine($"{i}-.  {p.Title}");
                    indexLookup[i] = p.Id;
                    i++;
                }

                Console.Write("\n Ingrese el indice de la propuesta para ver su estado: ");

                int seleccion;
                while (true)
                {
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out seleccion) && seleccion >= 1 && seleccion <= proyectos.Count)
                    {
                        break;
                    }
                    Console.WriteLine(" Selección no válida. Intente nuevamente o presione 'X' para salir al menú.");
                    if (input?.Trim().ToUpper() == "X")
                    {
                        return;
                    }
                }

                var id = indexLookup[seleccion];

                var propuesta = await _projectProposalService.GetProjectProposalByIdAsync(id);

                if (propuesta == null)
                {
                    Console.WriteLine(" Propuesta no encontrada.");
                    return;
                }

                ProposalSummaryPrinter.PrintOnlyData(propuesta);

                Console.WriteLine("\n Pasos actuales de Aprobación:");

                ProposalSummaryPrinter.PrintOnlyCurrentFlow(propuesta);
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Ocurrió un error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\n Presione cualquier tecla para regresar al menú...");
                Console.ReadKey();
            }
        }
    }
}
