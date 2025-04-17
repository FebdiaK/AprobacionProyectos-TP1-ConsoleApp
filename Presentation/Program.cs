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


namespace AprobacionProyectos.Presentation 
{
    class Program
    {
        private static IProjectProposalService _projectProposalService;   
        private static IUserService _userService; 
        


        static async Task Main(string[] args)
        {

            IServiceCollection services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AprobacionProyectos;Trusted_Connection=True;TrustServerCertificate=True;"));
 
            services.AddScoped<IApprovalRuleRepository, ApprovalRuleRepository>(); 
            services.AddScoped<IApprovalStatusRepository, ApprovalStatusRepository>();
            services.AddScoped<IApproverRoleRepository, ApproverRoleRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IProjectApprovalStepRepository, ProjectApprovalStepRepository>();
            services.AddScoped<IProjectProposalRepository, ProjectProposalRepository>();
            services.AddScoped<IProjectTypeRepository, ProjectTypeRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IProjectProposalService, ProjectProposalService>();
            services.AddScoped<IUserService, UserService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate(); // aplico las migraciones automáticamente
            }


            _projectProposalService = serviceProvider.GetRequiredService<IProjectProposalService>(); 
            _userService = serviceProvider.GetRequiredService<IUserService>();   

            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("\n===== MENÚ DE APROBACIÓN DE PROYECTOS =====");
                Console.WriteLine("1. Crear nueva propuesta");
                Console.WriteLine("2. Aprobar / Rechazar paso");
                Console.WriteLine("3. Ver estado de una propuesta");
                Console.WriteLine("0. Salir");
                Console.Write("Seleccione una opción: ");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        await CrearPropuestaDesdeConsolaAsync();
                        break;
                    case "2":
                        await AprobarPasoInteractivoAsync();
                        break;
                    case "3":
                        await VerEstadoPropuestaAsync();
                        break;
                    case "0":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("❌ Opción no válida.");
                        break;
                }
            }
        }

        internal static async Task CrearPropuestaDesdeConsolaAsync()
        {
            Console.Write(" Título del proyecto: ");
            var titulo = Console.ReadLine();
            Console.Write(" Descripción: ");
            var descripcion = Console.ReadLine();
            Console.Write(" ID del Área (1: Finanzas, 2: Tecnología, 3: RRHH, 4: Operaciones): ");
            var areaId = int.Parse(Console.ReadLine()!);
            Console.Write(" ID del Tipo de Proyecto (1: Mejora de Procesos, 2: Innovacion y Desarrollo, 3: Infraestructura, 4: Capacitación Interna): ");
            var tipoId = int.Parse(Console.ReadLine()!);
            Console.Write(" Monto estimado (en USD): ");
            var monto = decimal.Parse(Console.ReadLine()!);
            Console.Write(" Duración estimada (días): ");
            var duracion = int.Parse(Console.ReadLine()!);
            Console.Write(" ID del Usuario que crea la propuesta: ");
            var creadorId = int.Parse(Console.ReadLine()!);

            var propuesta = new ProjectProposal
            {
                Id = Guid.NewGuid(),
                Title = titulo!,
                Description = descripcion!,
                AreaId = areaId,
                TypeId = tipoId,
                EstimatedAmount = monto,
                EstimatedDuration = duracion,
                StatusId = 1,
                CreatedById = creadorId
            };

            var id = await _projectProposalService.CreateProjectProposalAsync(propuesta);   
            Console.WriteLine($" Propuesta creada con ID: {id}");
        }

        internal static async Task AprobarPasoInteractivoAsync()
        {
            Console.WriteLine(" Buscando proyectos pendientes de aprobación...");
            var proyectos = await _projectProposalService.GetAllProjectProposalsAsync(); 
            var pendientes = proyectos.Where(p => p.StatusId == 1).ToList();

            if (!pendientes.Any())
            {
                Console.WriteLine(" No hay proyectos pendientes.");
                return;
            }

            var indexLookup = new Dictionary<int, Guid>();
            int i = 1;

            foreach (var p in pendientes)
            {
                var pasoActual = p.ApprovalSteps.OrderBy(s => s.StepOrder).FirstOrDefault(s => s.StatusId == 1);
                Console.WriteLine($"{i}. {p.Title} | Paso actual: {pasoActual?.ApproverRole.Name} (Orden #{pasoActual?.StepOrder})");
                indexLookup[i] = p.Id;
                i++;
            }

            Console.Write("Seleccione el número de proyecto: ");
            var seleccion = int.Parse(Console.ReadLine()!);
            var id = indexLookup[seleccion];

            var propuesta = await _projectProposalService.GetProjectProposalByIdAsync(id); // Use field  
            if (propuesta == null || propuesta.StatusId != 1)
            {
                Console.WriteLine(" Propuesta no encontrada o ya procesada.");
                return;
            }

            var paso = propuesta.ApprovalSteps.OrderBy(s => s.StepOrder).FirstOrDefault(s => s.StatusId == 1);
            if (paso == null)
            {
                Console.WriteLine("No hay paso pendiente.");
                return;
            }

            Console.Write($" Ingrese su ID de usuario (para rol '{paso.ApproverRole.Name}'): ");
            var userId = int.Parse(Console.ReadLine()!);

            if (paso.UserId != null && paso.UserId != userId)
            {
                Console.WriteLine("❌ Este paso no está asignado a usted.");
                return;
            }

            Console.Write(" ¿Desea aprobar el paso? (S/N): ");
            var aprobar = Console.ReadLine()?.Trim().ToUpper() == "S";
            Console.Write(" Observaciones (opcional): ");
            var obs = Console.ReadLine();

            var resultado = await _projectProposalService.ApproveStepAsync(paso.Id, userId, aprobar, obs);   
            Console.WriteLine(resultado ? " Paso procesado correctamente." : "❌ Error en la aprobación.");
        }

        internal static async Task VerEstadoPropuestaAsync()
        {
            Console.Write(" Ingrese el ID de la propuesta: ");
            var input = Console.ReadLine();

            if (Guid.TryParse(input, out var proposalId))
            {
                var proposal = await _projectProposalService.GetProjectProposalByIdAsync(proposalId);   

                if (proposal == null)
                {
                    Console.WriteLine("❌ Propuesta no encontrada.");
                    return;
                }

                Console.WriteLine($"\n Título: {proposal.Title}");
                Console.WriteLine($" Estado: {proposal.Status.Name}");
                Console.WriteLine($" Área: {proposal.Area.Name}");
                Console.WriteLine($" Tipo: {proposal.Type.Name}");
                Console.WriteLine($" Monto estimado: ${proposal.EstimatedAmount}");
                Console.WriteLine($" Duración estimada: {proposal.EstimatedDuration} días");
                Console.WriteLine($" Creado por: {proposal.CreatedBy.Name}");
                Console.WriteLine($" Fecha: {proposal.CreatedAt}");

                Console.WriteLine("\n Pasos de Aprobación:");
                foreach (var step in proposal.ApprovalSteps.OrderBy(s => s.StepOrder))
                {
                    var estado = step.Status.Name;
                    var fecha = step.DecisionDate?.ToString("dd/MM/yyyy HH:mm") ?? "Pendiente";
                    var observacion = string.IsNullOrWhiteSpace(step.Observations) ? "-" : step.Observations;

                    Console.WriteLine($" - Paso #{step.StepOrder} | Rol: {step.ApproverRole.Name} | Estado: {estado} | Fecha: {fecha} | Obs: {observacion}");
                }
            }
            else
            {
                Console.WriteLine(" ID inválido.");
            }
        }
    } }

