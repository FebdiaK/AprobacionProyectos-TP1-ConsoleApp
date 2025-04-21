using AprobacionProyectos.Models;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Data.Seeders
{
    public static class UserSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "José Ferreyra", Email = "jferreyra@unaj.com", RoleId = 2 },
                new User { Id = 2, Name = "Ana Lucero", Email = "alucero@unaj.com", RoleId = 1 },
                new User { Id = 3, Name = "Gonzalo Molinas", Email = "gmolinas@unaj.com", RoleId = 2 },
                new User { Id = 4, Name = "Lucas Olivera", Email = "lolivera@unaj.com", RoleId = 3 },
                new User { Id = 5, Name = "Danilo Fagundez", Email = "dfagundez@unaj.com", RoleId = 4 },
                new User { Id = 6, Name = "Gabriel Galli", Email = "ggalli@unaj.com", RoleId = 4 }
            );
        }
    }
}
