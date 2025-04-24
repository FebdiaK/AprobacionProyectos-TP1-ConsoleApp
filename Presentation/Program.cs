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
using Microsoft.Extensions.DependencyInjection;
using AprobacionProyectos.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using AprobacionProyectos.Presentation.MenuActions;
using AprobacionProyectos.Presentation.Helpers;
using AprobacionProyectos.Presentation;


IServiceCollection services = new ServiceCollection(); //configuro el contenedor de servicios

//registro DbContext con Localdb 
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AprobacionProyectos;Trusted_Connection=True;TrustServerCertificate=True;"));

//repositorios (interfaces e implementaciones)
services.AddScoped<IApprovalRuleRepository, ApprovalRuleRepository>();
services.AddScoped<IApprovalStatusRepository, ApprovalStatusRepository>();
services.AddScoped<IApproverRoleRepository, ApproverRoleRepository>();
services.AddScoped<IAreaRepository, AreaRepository>();
services.AddScoped<IProjectApprovalStepRepository, ProjectApprovalStepRepository>();
services.AddScoped<IProjectProposalRepository, ProjectProposalRepository>();
services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
services.AddScoped<IUserRepository, UserRepository>();

//servicios de aplicacion
services.AddScoped<IApprovalStatusService, ApprovalStatusService>();
services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();
services.AddScoped<IAreaService, AreaService>();
services.AddScoped<IProjectProposalCreatorService, ProjectProposalCreatorService>();
services.AddScoped<IProjectProposalQueryService, ProjectProposalQueryService>();
services.AddScoped<IProjectTypeService, ProjectTypeService>();
services.AddScoped<IUserService, UserService>();

//acciones del menu
services.AddScoped<ApprovalConfirmationHelper>();
services.AddScoped<InputValidators>();
services.AddScoped<ProjectSelecionFromListHelper>();
services.AddScoped<ProjectPendingSelectionHelper>();
services.AddScoped<ProposalBuilder>();
services.AddScoped<ProposalSummaryPrinter>();
services.AddScoped<UserValidationHelper>();
services.AddScoped<ApproveStepAction>();
services.AddScoped<CreateProposalAction>();
services.AddScoped<ViewProposalStatusAction>();
services.AddScoped<ConsoleMenuService>();

//build y migracion de base de datos 
ServiceProvider serviceProvider = services.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // aplico las migraciones automáticamente
}

//ejectuo el menú principal
var menu = serviceProvider.GetRequiredService<ConsoleMenuService>();
await menu.RunAsync();
