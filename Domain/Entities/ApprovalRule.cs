using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Domain.Entities
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
        public int? AreaId { get; set; } // foreign key del Area, puede ser null(por eso el '?')
        public Area? Area { get; set; } // relación con Area, puede ser null


        [ForeignKey(nameof(Type))]
        [Column("Type")] 
        public int? TypeId { get; set; } 
        public ProjectType? Type { get; set; } 


        [NotNull]
        public required int StepOrder { get; set; }


        [ForeignKey(nameof(ApproverRole))]
        [Column("ApproverRoleId")] 
        public int ApproverRoleId { get; set; } 
        public ApproverRole ApproverRole { get; set; } = null!; //no puede ser null;
    }
}
