using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using AprobacionProyectos.Presentation.Helpers;

namespace AprobacionProyectos.Presentation.Helpers
{
    public class ProposalBuilder
    {
        private readonly InputValidators _inputValidators; 
        private readonly IAreaService _areaService;
        private readonly IProjectTypeService _projectTypeService; 


        public ProposalBuilder(InputValidators inputValidators,  IAreaService areaService, IProjectTypeService projectTypeService)
        {
            _inputValidators = inputValidators;
            _areaService = areaService;
            _projectTypeService = projectTypeService;
        }

        public async Task<ProjectProposal> BuildAsync()
        {
            Console.WriteLine("Ingrese los detalles de la propuesta de Proyecto o ingrese 'x' para salir:");
            string LeerEntrada(string mensaje, Func<string, bool> validador) //funcion que en base a lo que se ingrese, se valida si es válida o no la entrada
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
           
            var areas = await _areaService.GetAllAreasAsync();
            var areaId = int.Parse(LeerEntrada(" ID del Área (1-" + areas.Count + "): ", entrada => InputValidators.ValidarRango(entrada, 1, areas.Count()))); //se valida el rango de areas dinámicamente 
            
            var tipos = await _projectTypeService.GetAllProjectTypesAsync();
            var tipoId = int.Parse(LeerEntrada(" ID del Tipo de Proyecto (1-" + tipos.Count + "): ", entrada => InputValidators.ValidarRango(entrada, 1, tipos.Count())));

            var monto = decimal.Parse(LeerEntrada(" Monto estimado (en USD): ", entrada => InputValidators.ValidarDecimalPositivo(entrada)));
            var duracion = int.Parse(LeerEntrada(" Duración estimada (días): ", entrada => InputValidators.ValidarEnteroPositivo(entrada)));
            var creadorId = await _inputValidators.LeerYValidarUsuarioAsync();
           
            if (creadorId == null)
            {
                Console.WriteLine(" - Se canceló la creacion del proyecto.");
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
