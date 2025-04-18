using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Services;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Repositories.Interfaces;
using AprobacionProyectos.Infrastructure.Repositories.Implementations;
using AprobacionProyectos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Presentation.Helpers;

namespace AprobacionProyectos.Presentation.MenuActions
{
    public class CreateProposalAction
    {
        private readonly IProjectProposalService _projectProposalService; 
        private readonly IUserService _userService;
        private readonly ProposalBuilder _proposalBuilder; 

        public CreateProposalAction(IProjectProposalService service, IUserService userService, ProposalBuilder proposalBuilder)
        {
            _projectProposalService = service;
            _userService = userService;
            _proposalBuilder = proposalBuilder;
        }

        public async Task RunAsync()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("===== CREAR NUEVA PROPUESTA =====");

                var propuesta = await _proposalBuilder.BuildAsync();

                var id = await _projectProposalService.CreateProjectProposalAsync(propuesta);
                var fullProposal = await _projectProposalService.GetProjectProposalFullWithId(id);

                if (fullProposal == null)
                {
                    Console.WriteLine(" Ocurrió un error al recuperar la propuesta completa.");
                    return;
                }

                ProposalSummaryPrinter.PrintData(fullProposal);
                Console.WriteLine($"\n - Propuesta creada con ID: {id}");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n Operación cancelada. Regresando al menú...");
            }
            catch (FormatException)
            {
                Console.WriteLine(" Entrada inválida. Asegúrese de ingresar los datos en el formato correcto.");
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
