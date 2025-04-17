using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AprobacionProyectos.Domain.Entities
{ 
        public class Area
        {
            [Key]
            public int Id { get; set; }

            [MaxLength(25)]
            public string Name { get; set; } = null!;

            public ICollection<ProjectProposal> ProjectProposals { get; set; } = new List<ProjectProposal>();
            public ICollection<ApprovalRule> ApprovalRules { get; set; } = new List<ApprovalRule>();

    }
  
}
