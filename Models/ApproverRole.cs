using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Models
{
    public class ApproverRole
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(25)]
        public string Name { get; set; } = null!;

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<ApprovalRule> ApprovalRules { get; set; } = new List<ApprovalRule>();
        public ICollection<ProjectApprovalStep> ProjectApprovalSteps { get; set; } = new List<ProjectApprovalStep>();

    }
}
