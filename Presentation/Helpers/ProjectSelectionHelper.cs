using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Presentation.Helpers
{
    public class ProjectSelectionHelper
    {
        public static ProjectProposal SelectProposal(List<ProjectProposal> propuestas)
        {
            var pendientes = propuestas.Where(p => p.StatusId == 1).ToList();

            if (pendientes.Count == 0)
            {
                Console.WriteLine(" No hay proyectos pendientes.");
                return null;
            }

            Console.WriteLine("\n Proyectos pendientes: ");
            var indexLookup = new Dictionary<int, Guid>(); // utilizamos un diccionario para mapear el número de proyecto con su Guid
            int i = 1; 

            foreach (var p in pendientes) // recorremos la lista de propuestas pendientes
            {
                var pasoActual = p.ApprovalSteps.OrderBy(s => s.StepOrder).FirstOrDefault(s => s.StatusId == 1); // obtenemos el paso actual del proyecto
                Console.WriteLine($"{i}. {p.Title} | Paso actual: {pasoActual?.ApproverRole.Name} (Orden #{pasoActual?.StepOrder})");
                indexLookup[i] = p.Id; // guardamos el id del proyecto en el diccionario
                i++; 
            }

            Console.Write("\nSeleccione el número de proyecto o presione 'x' para salir: ");
            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out int seleccion) && indexLookup.ContainsKey(seleccion)) // si la entrada es un número y se encuentra en el diccionario
                {
                    var id = indexLookup[seleccion]; // obtenemos el id del proyecto
                    return propuestas.First(p => p.Id == id); // retornamos el proyecto 
                }

                if (input?.Trim().ToUpper() == "X") // si la entrada es 'X', salimos del menú
                    return null!;

                Console.WriteLine(" Selección no válida. Intente nuevamente o presione 'X' para salir al menú.");
            }
        }
    }
}
