using AprobacionProyectos.Models;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Data.Seeders
{
    public static class ApprovalStatusSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatus>().HasData(
                new ApprovalStatus { Id = 1, Name = "Pending" },
                new ApprovalStatus { Id = 2, Name = "Approved" },
                new ApprovalStatus { Id = 3, Name = "Rejected" },
                new ApprovalStatus { Id = 4, Name = "Observed" }
            );
        }
    }
}
