using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Domain.Entities
{
    public class ProjectProposal
    {
        [Key]
        public required Guid Id { get; set; }

        [MaxLength(255)]
        public string Title { get; set; } = null!;

        [Column(TypeName = "varchar(max)")]
        public string Description { get; set; } = null!;

        [ForeignKey("Area")]
        public int AreaId { get; set; } // Foreign key del Area
        public Area Area { get; set; } = null!; // Relación con Area

        [ForeignKey("Type")]
        public int TypeId { get; set; } // Foreign key del ProyectType
        public ProjectType Type { get; set; } = null!; // Relación con ProyectType

        public decimal EstimatedAmount { get; set; }  // monto estimado del proyecto
        public int EstimatedDuration { get; set; }  // en dias segun el tp

        [ForeignKey("Status")]
        public int StatusId { get; set; } // Foreign key del ApprovalStatus
        public ApprovalStatus Status { get; set; } = null!; // Relación con ApprovalStatus

        public DateTime CreatedAt { get; set; }

        [ForeignKey("CreatedBy")]
        public int CreatedById { get; set; } // Foreign key del User
        public User CreatedBy { get; set; } = null!; // Relación con User

        public ICollection<ProjectApprovalStep> ApprovalSteps { get; set; } = new List<ProjectApprovalStep>();

    }
}
