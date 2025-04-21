using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Presentation.Helpers
{
    public class UserValidationHelper
    {
        public static async Task<User?> GetValidUserAsync(IUserService userService, ProjectApprovalStep paso)
        {
            while (true)
            {
                Console.Write($"\nIngrese su ID del usuario (para rol '{paso.ApproverRole.Name}') o presione 'X' para salir: ");
                var input = Console.ReadLine();
                if (input?.Trim().ToUpper() == "X")
                {
                    return null;
                }

                if (!int.TryParse(input, out int userId))
                {
                    Console.WriteLine("ID inválido. Intente nuevamente.");
                    continue;
                }

                var user = await userService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    Console.WriteLine($"Usuario con ID {userId} no encontrado.");
                    Console.WriteLine("Intente de nuevo.");
                    continue;
                }

                if (user.ApproverRole?.Id != paso.ApproverRole.Id)
                {
                    Console.WriteLine($"\nEl usuario no tiene el rol necesario para aprobar este paso. Rol requerido: {paso.ApproverRole.Name}");
                    Console.WriteLine("Intente de nuevo.");
                    continue;
                }

                Console.WriteLine($"\nUsuario encontrado: {user.Name}, Rol: {user.ApproverRole.Name}");
                return user;
            }
        }
    }
}
