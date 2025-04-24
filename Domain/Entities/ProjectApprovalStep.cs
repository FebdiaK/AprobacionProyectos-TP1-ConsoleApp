using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Domain.Entities
{
    public class ProjectApprovalStep
    {
        [Key]
        public long Id { get; set; }


        [ForeignKey(nameof(ProjectProposal))]
        public Guid ProjectProposalId { get; set; } 
        public ProjectProposal ProjectProposal { get; set; } = null!; 


        [ForeignKey (nameof(ApproverUser))]
        [Column("ApproverUserId")] 
        public int? ApproverUserId { get; set; }  
        public User? ApproverUser { get; set; } 


        [ForeignKey(nameof(ApproverRole))]
        [Column("ApproverRoleId")] 
        public int ApproverRoleId { get; set; } 
        public ApproverRole ApproverRole { get; set; } = null!;


        [ForeignKey(nameof(Status))]
        [Column("Status")] 
        public int StatusId { get; set; }  
        public ApprovalStatus Status { get; set; } = null!; 


        public required int StepOrder { get; set; } 
        
        
        public DateTime? DecisionDate { get; set; } 


        [Column(TypeName = "varchar(max)")]
        public string? Observations { get; set; } 

    }
}
