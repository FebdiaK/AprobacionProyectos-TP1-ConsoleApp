using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Data.Seeders
{
    public static class ApproverRoleSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApproverRole>().HasData(
                new ApproverRole { Id = 1, Name = "Líder de Área" },
                new ApproverRole { Id = 2, Name = "Gerente" },
                new ApproverRole { Id = 3, Name = "Director" },
                new ApproverRole { Id = 4, Name = "Comité Técnico" }
            );
        }
    }
}
