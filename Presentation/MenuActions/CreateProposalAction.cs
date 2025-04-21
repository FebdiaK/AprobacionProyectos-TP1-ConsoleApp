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
        private readonly IProjectProposalCreatorService _creatorService;
        private readonly IProjectProposalQueryService _queryService;
        private readonly ProposalBuilder _proposalBuilder;
        private readonly ProposalSummaryPrinter _proposalSummaryPrinter;
        public CreateProposalAction(IProjectProposalCreatorService creatorService, IProjectProposalQueryService queryService, ProposalBuilder proposalBuilder, ProposalSummaryPrinter proposalSummaryPrinter)
        {
            _creatorService = creatorService;
            _queryService = queryService;
            _proposalBuilder = proposalBuilder;
            _proposalSummaryPrinter = proposalSummaryPrinter;
        }

        public async Task RunAsync()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("===== CREAR NUEVA PROPUESTA =====");

                var propuesta = await _proposalBuilder.BuildAsync();

                var id = await _creatorService.CreateProjectProposalAsync(propuesta); //esta linea de codio me genera una excepcion:  An error occurred while saving the entity changes. See the inner exception for details.

                var fullProposal = await _queryService.GetProjectProposalFullWithId(id);

                if (fullProposal == null)
                {
                    Console.WriteLine(" Ocurrió un error al recuperar la propuesta completa.");
                    return;
                }

                _proposalSummaryPrinter.PrintData(fullProposal);
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
                Console.WriteLine(ex.InnerException.Message);
                throw;
            }
            finally
            {
                Console.WriteLine("\n Presione cualquier tecla para regresar al menú...");
                Console.ReadKey();
            }
        }
    }
}
