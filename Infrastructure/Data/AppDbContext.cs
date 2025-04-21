using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Infrastructure.Data.Configurations;
using AprobacionProyectos.Domain.Entities;
using AprobacionProyectos.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;

namespace AprobacionProyectos.Infrastructure.Data
{
    public class AppDbContext : DbContext
    { 

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ApprovalRule> ApprovalRules { get; set; }
        public DbSet<ApprovalStatus> ApprovalStatuses { get; set; }
        public DbSet<ApproverRole> ApproverRoles { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<ProjectApprovalStep> ProjectApprovalSteps { get; set; }
        public DbSet<ProjectProposal> ProjectProposals { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // configuraciones Fluent (para evitar elimin en cascada)  
            modelBuilder.ApplyConfiguration(new ProjectApprovalStepConfig());
            modelBuilder.ApplyConfiguration(new ProjectProposalConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());

            // seeders  
            ApprovalRuleSeeder.Seed(modelBuilder);
            ApprovalStatusSeeder.Seed(modelBuilder);
            ApproverRoleSeeder.Seed(modelBuilder);
            AreaSeeder.Seed(modelBuilder);
            ProjectTypeSeeder.Seed(modelBuilder);
            UserSeeder.Seed(modelBuilder);
        }
    }
}
