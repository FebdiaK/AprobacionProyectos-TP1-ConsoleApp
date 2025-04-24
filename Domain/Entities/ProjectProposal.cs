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
        public int AreaId { get; set; } 
        public Area Area { get; set; } = null!; 

        [ForeignKey("Type")]
        public int TypeId { get; set; } 
        public ProjectType Type { get; set; } = null!; 

        public decimal EstimatedAmount { get; set; }  
        public int EstimatedDuration { get; set; }  

        [ForeignKey("Status")]
        public int StatusId { get; set; } 
        public ApprovalStatus Status { get; set; } = null!; 

        public DateTime CreatedAt { get; set; }

        [ForeignKey("CreatedBy")]
        public int CreatedById { get; set; } 
        public User CreatedBy { get; set; } = null!; 

        public ICollection<ProjectApprovalStep> ApprovalSteps { get; set; } = new List<ProjectApprovalStep>();

    }
}
