using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Presentation.Helpers
{
    public class ProjectSelecionFromListHelper
    {
        public static ProjectProposal? SelectProjectFromList(List<ProjectProposal> propuestas)
        {
            if (propuestas.Count == 0) 
            {
                Console.WriteLine(" No hay proyectos disponibles.");
                return null;
            }

            Console.WriteLine(" Lista de propuestas de Proyectos: \n");
            var indexLookup = new Dictionary<int, Guid>();
            int i = 1;

            foreach (var p in propuestas)
            {
                Console.WriteLine($"{i}-.  {p.Title}");
                indexLookup[i] = p.Id;
                i++;
            }

            Console.Write("\n Ingrese el índice de la propuesta para ver su estado o presione 'X' para salir: ");

            while (true)
            {
                var input = Console.ReadLine();
                if (int.TryParse(input, out int seleccion) && indexLookup.ContainsKey(seleccion))
                    return propuestas.First(p => p.Id == indexLookup[seleccion]);

                if (input?.Trim().ToUpper() == "X")
                    return null;

                Console.WriteLine(" Selección no válida. Intente nuevamente o presione 'X' para salir al menú.");
            }
        }
    }
}
