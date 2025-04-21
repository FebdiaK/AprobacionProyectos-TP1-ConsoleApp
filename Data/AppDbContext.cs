using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Data.Configurations;
using AprobacionProyectos.Data.Seeders;
using AprobacionProyectos.Models;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=AprobacionProyectos;Trusted_Connection=True;");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<ApproverRole> ApproverRoles { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<ApprovalRule> ApprovalRules { get; set; }
        public DbSet<ProjectProposal> ProjectProposals { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
        public DbSet<ProjectApprovalStep> ProjectApprovalSteps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones Fluent (para evitar elimin en cascada)
            modelBuilder.ApplyConfiguration(new ProjectApprovalStepConfig());
            modelBuilder.ApplyConfiguration(new ProjectProposalConfig());

            //Seeders
            AreaSeeder.Seed(modelBuilder); 
            ProjectTypeSeeder.Seed(modelBuilder);
            ApprovalStatusSeeder.Seed(modelBuilder);
            ApproverRoleSeeder.Seed(modelBuilder);
            UserSeeder.Seed(modelBuilder);
            ApprovalRuleSeeder.Seed(modelBuilder);
        }
    }
}
