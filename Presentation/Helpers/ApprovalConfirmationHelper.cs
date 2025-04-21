using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Presentation.Helpers
{
    public class ApprovalConfirmationHelper
    {
        public static (bool aprobado, string? observaciones)? GetDecision()
        {
            while (true)
            {
                Console.Write("\n¿Desea aprobar el paso? (S/N) u oprima 'x' para salir: ");
                var input = Console.ReadLine();
                if (input?.Trim().ToUpper() == "X")
                {
                    return null; 
                }

                if (input?.ToUpper() == "S" || input?.ToUpper() == "N")
                {
                    bool aprobado = input?.ToUpper() == "S";

                    Console.Write("-Observaciones (opcional): ");
                    var obs = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(obs))
                    {
                        obs = null;
                    }

                    return (aprobado, obs);
                }

                Console.WriteLine(" Opción inválida. Por favor, ingrese una opción válida.");
            }
        }
    }
}
