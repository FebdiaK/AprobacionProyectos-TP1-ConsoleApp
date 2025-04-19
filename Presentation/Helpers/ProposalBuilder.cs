using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Presentation.Helpers;

namespace AprobacionProyectos.Presentation.Helpers
{
    public class ProposalBuilder
    {
        private readonly IUserService _userService;
        private readonly InputValidators _inputValidators;

        public ProposalBuilder(IUserService userService, InputValidators inputValidators)
        {
            _userService = userService;
            _inputValidators = inputValidators;
        }

        public async Task<ProjectProposal> BuildAsync()
        {
            Console.WriteLine("Ingrese los detalles de la propuesta de Proyecto o ingrese 'x' para salir:");
            string LeerEntrada(string mensaje, Func<string, bool> validador)
            {
                string? entrada;
                do
                {
                    Console.Write(mensaje);
                    entrada = Console.ReadLine();
                    if (entrada?.ToLower() == "x") throw new OperationCanceledException();
                    if (validador(entrada)) return entrada!;
                    Console.WriteLine(" Entrada inválida. Intente nuevamente.");
                } while (true);
            }

            var titulo = LeerEntrada(" Título del proyecto: ", entrada => !string.IsNullOrWhiteSpace(entrada));
            var descripcion = LeerEntrada(" Descripción: ", entrada => !string.IsNullOrWhiteSpace(entrada));
            var areaId = int.Parse(LeerEntrada(" ID del Área (1-4): ", entrada => InputValidators.ValidarRango(entrada, 1, 4)));
            var tipoId = int.Parse(LeerEntrada(" ID del Tipo de Proyecto (1-4): ", entrada => InputValidators.ValidarRango(entrada, 1, 4)));
            var monto = decimal.Parse(LeerEntrada(" Monto estimado (en USD): ", entrada => InputValidators.ValidarDecimalPositivo(entrada)));
            var duracion = int.Parse(LeerEntrada(" Duración estimada (días): ", entrada => InputValidators.ValidarEnteroPositivo(entrada)));
            var creadorId = await _inputValidators.LeerYValidarUsuarioAsync();
           
            if (creadorId == null)
            {
                Console.WriteLine(" Se canceló la creacion del proyecto.");
                return null;
            }

            return new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = titulo,
                Description = descripcion,
                AreaId = areaId,
                TypeId = tipoId,
                EstimatedAmount = monto,
                EstimatedDuration = duracion,
                StatusId = 1,
                CreatedById = (int)creadorId
            };
        }
    }
}
