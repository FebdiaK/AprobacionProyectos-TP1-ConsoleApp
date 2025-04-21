using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Models
{
    public class ApprovalRule
    {
        [Key]
        public required long Id { get; set; } // bigint en C# es un long
        [NotNull]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinAmount { get; set; }
        [NotNull]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MaxAmount { get; set; } 


        [ForeignKey(nameof(Area))]
        [Column("Area")] // especifico el nombre de la columna en la base de datos
        public int? AreaId { get; set; } // Foreign key del Area, puede ser null(por eso el '?')
        public Area? Area { get; set; } // Relación con Area, puede ser null


        [ForeignKey(nameof(Type))]
        [Column("Type")] // especifico el nombre de la columna en la base de datos
        public int? TypeId { get; set; } // Foreign key del ProyectType, puede ser null
        public ProjectType? Type { get; set; } // Relación con ProyectType, puede ser null


        [NotNull]
        public required int StepOrder { get; set; }


        [ForeignKey(nameof(ApproverRole))]
        [Column("ApproverRoleId")] // especifico el nombre de la columna en la base de datos
        public int ApproverRoleId { get; set; } // Foreign key del ApproverRole
        public ApproverRole ApproverRole { get; set; } = null!; // Relación con ApproverRole
    }
}
