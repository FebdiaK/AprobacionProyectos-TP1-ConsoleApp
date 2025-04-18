using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Presentation.MenuActions
{
    public class ApproveStepAction
    {
        private readonly IProjectProposalService _projectProposalService;
        private readonly IUserService _userService;

        public ApproveStepAction(IProjectProposalService projectProposalService, IUserService userService)
        {
            _projectProposalService = projectProposalService;
            _userService = userService;
        }

        public async Task RunAsync()
        {
            try
            {

                Console.Clear();
                Console.WriteLine("===== APROBAR / RECHAZAR PASO =====");

                Console.WriteLine(" Buscando proyectos pendientes de aprobación...");
                var proyectos = await _projectProposalService.GetAllProjectProposalsAsync();
                var pendientes = proyectos.Where(p => p.StatusId == 1).ToList();

                if (pendientes.Count == 0)
                {
                    Console.WriteLine(" No hay proyectos pendientes.");
                    return;
                }

                Console.WriteLine("\n Proyectos pendientes: ");
                var indexLookup = new Dictionary<int, Guid>();
                int i = 1;

                foreach (var p in pendientes)
                {
                    var pasoActual = p.ApprovalSteps.OrderBy(s => s.StepOrder).FirstOrDefault(s => s.StatusId == 1);
                    Console.WriteLine($"{i}. {p.Title} | Paso actual: {pasoActual?.ApproverRole.Name} (Orden #{pasoActual?.StepOrder})");
                    indexLookup[i] = p.Id;
                    i++;
                }

                Console.Write("Seleccione el número de proyecto: ");
                int seleccion;
                while (true)
                {
                    var input = Console.ReadLine();
                    if (int.TryParse(input, out seleccion) && seleccion >= 1 && seleccion <= pendientes.Count)
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

                if (propuesta == null || propuesta.StatusId != 1)
                {
                    Console.WriteLine(" Propuesta no encontrada o ya procesada.");
                    return;
                }

                var paso = propuesta.ApprovalSteps.OrderBy(s => s.StepOrder).FirstOrDefault(s => s.StatusId == 1);
                if (paso == null)
                {
                    Console.WriteLine("No hay paso pendiente.");
                    return;
                }

                Console.Write($" Ingrese su ID de usuario (para rol '{paso.ApproverRole.Name}'): ");
                var userId = int.Parse(Console.ReadLine()!);
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"Usuario con ID {userId} no encontrado en la base de datos.");
                    return;
                }
                if (user.ApproverRole == null)
                {
                    Console.WriteLine($"El usuario con ID {userId} no tiene un rol asignado.");
                    return;
                }
                Console.WriteLine($"Usuario encontrado: {user.Name}, Rol: {user.ApproverRole.Name}");

                if (paso.UserId != null && paso.UserId != userId)
                {
                    Console.WriteLine(" Este paso no está asignado a usted.");
                    return;
                }

                Console.Write(" ¿Desea aprobar el paso? (S/N): ");
                var aprobar = Console.ReadLine()?.Trim().ToUpper() == "S";
                Console.Write(" Observaciones (opcional): ");
                var obs = Console.ReadLine();

                var resultado = await _projectProposalService.ApproveStepAsync(paso.Id, userId, aprobar, obs);
                Console.WriteLine(resultado ? " Paso procesado correctamente." : " Error en la aprobación.");
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
