using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;

namespace AprobacionProyectos.Presentation.Helpers 
{
    public class InputValidators
    {
        private readonly IUserService _userService;

        public InputValidators(IUserService userService)
        {
            _userService = userService;
        }

        public static bool ValidarRango(string entrada, int min, int max)
        {
            return int.TryParse(entrada, out var val) && val >= min && val <= max;
        }

        public static bool ValidarDecimalPositivo(string entrada)
        {
            return decimal.TryParse(entrada, out var val) && val > 0;
        }

        public static bool ValidarEnteroPositivo(string entrada)
        {
            return int.TryParse(entrada, out var val) && val > 0;
        }

        public async Task<int?> LeerYValidarUsuarioAsync()
        {
            while (true)
            {
            
                Console.WriteLine("Ingrese el ID del usuario (o '000' para cancelar):");
                var entrada = Console.ReadLine();
          
                if (entrada == "000")
                {
                    return null; // Indica que se canceló la operación
                }

                if (!int.TryParse(entrada, out var _id))
                {
                    Console.WriteLine("Entrada inválida. Intente nuevamente.");
                    continue;
                }

                try
                {   
                    var usuario = await _userService.GetUserByIdAsync(_id);

                    if (usuario == null)
                    {
                        Console.WriteLine("Usuario no encontrado. Intente nuevamente.");
                        continue;
                    }

                    return usuario.Id; // Usuario válido encontrado
                }
                catch
                {
                    Console.WriteLine("Error al buscar el usuario. Intente nuevamente.");
                }
            
            }
        }
    }
}

