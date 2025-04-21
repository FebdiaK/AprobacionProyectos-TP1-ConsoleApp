using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Models
{
    public class ApprovalStatus
    {
        [Key]
        public required int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; } = null!;

        public ICollection<ProjectProposal> ProjectProposals { get; set; } = new List<ProjectProposal>();
        public ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; } = new List<ProjectApprovalStep>();
    }
}
