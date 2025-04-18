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
        public long Id { get; set; } // bigint en C# es un long


        [ForeignKey(nameof(ProjectProposal))]
        public Guid ProjectProposalId { get; set; } // Foreign key del ProjectProposal
        public ProjectProposal ProjectProposal { get; set; } = null!; // relacion con ProjectProposal


        [ForeignKey(nameof(User))]
        [Column("ApproverUserId")] // especifico el nombre de la columna en la base de datos
        public int? UserId { get; set; } // Foreign key del User, puede ser null
        public User? User { get; set; } // relacion con User, puede ser null


        [ForeignKey(nameof(ApproverRole))]
        [Column("ApproverRoleId")] // especifico el nombre de la columna en la base de datos
        public int ApproverRoleId { get; set; } // Foreign key del ApproverRole
        public ApproverRole ApproverRole { get; set; } = null!; // relacion con ApproverRole


        [ForeignKey(nameof(Status))]
        [Column("Status")] // especifico el nombre de la columna en la base de datos
        public int StatusId { get; set; } // Foreign key del ApprovalStatus
        public ApprovalStatus Status { get; set; } = null!; // relacion con ApprovalStatus


        public required int StepOrder { get; set; } // orden del paso de aprobacion
        
        
        public DateTime? DecisionDate { get; set; } // fecha de creacion del paso de aprobacion


        [Column(TypeName = "varchar(max)")]
        public string? Observations { get; set; } // observaciones del paso de aprobacion

    }
}
