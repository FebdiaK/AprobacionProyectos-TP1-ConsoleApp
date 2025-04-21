using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AprobacionProyectos.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AprobacionProyectos.Infrastructure.Data.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .HasOne(u => u.ApproverRole) // defino relación con ApproverRole, porque un user puede tener un role y un role puede tener muchos users;
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
        }
    }
}

