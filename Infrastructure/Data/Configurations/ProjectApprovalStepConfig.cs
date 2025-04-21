using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AprobacionProyectos.Domain.Entities;

namespace AprobacionProyectos.Infrastructure.Data.Configurations
{
    public class ProjectApprovalStepConfig : IEntityTypeConfiguration<ProjectApprovalStep>
    {
        public void Configure(EntityTypeBuilder<ProjectApprovalStep> builder)
        {
            builder.HasOne(p => p.ProjectProposal) // relacion con ProjectProposal, 
                   .WithMany(p => p.ApprovalSteps) // relacion inversa con ProjectProposal
                   .HasForeignKey(p => p.ProjectProposalId) // foreign key del ProjectProposal
                   .OnDelete(DeleteBehavior.Restrict); // cambiado a Restrict para evitar la eliminación en cascada

            builder.HasOne(p => p.ApproverUser)
                   .WithMany(u => u.ApprovalSteps)
                   .HasForeignKey(p => p.ApproverUserId) // foreign key del User que aprueba
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ApproverRole)
                   .WithMany(r => r.ProjectApprovalSteps)
                   .HasForeignKey(p => p.ApproverRoleId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Status)
                   .WithMany(s => s.ProjectApprovalSteps)
                   .HasForeignKey(p => p.StatusId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

