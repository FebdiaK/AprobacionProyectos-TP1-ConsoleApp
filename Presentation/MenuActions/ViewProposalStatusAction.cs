using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Application.Interfaces;
using AprobacionProyectos.Presentation.Helpers;

namespace AprobacionProyectos.Presentation.MenuActions
{
    public class ViewProposalStatusAction
    {
        private readonly IProjectProposalQueryService _projectProposalQueryService;
        private readonly ProposalSummaryPrinter _proposalSummaryPrinter;
        public ViewProposalStatusAction( ProposalSummaryPrinter proposalSummaryPrinter, IProjectProposalQueryService projectProposalQueryService)
        {
            _proposalSummaryPrinter = proposalSummaryPrinter;
            _projectProposalQueryService = projectProposalQueryService;
        }

        public async Task RunAsync()
        {
            try
            {
                Console.Clear();

                Console.WriteLine("===== VER ESTADO DE UNA PROPUESTA =====");

                var proyectos = await _projectProposalQueryService.GetAllProjectProposalsAsync();

                var propuesta = ProjectSelecionFromListHelper.SelectProjectFromList(proyectos); 
                if (propuesta == null) 
                    return ;

                _proposalSummaryPrinter.PrintOnlyData(propuesta);

                await _proposalSummaryPrinter.PrintOnlyCurrentFlow(propuesta);
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
