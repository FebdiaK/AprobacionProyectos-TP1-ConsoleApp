using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Data.Seeders
{
    public static class ProjectTypeSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProjectType>().HasData(
                new ProjectType { Id = 1, Name = "Mejora de Procesos" },
                new ProjectType { Id = 2, Name = "Innovación y Desarrollo" },
                new ProjectType { Id = 3, Name = "Infraestructura" },
                new ProjectType { Id = 4, Name = "Capacitación Interna" }
            );
        }
    }
}
