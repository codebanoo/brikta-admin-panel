using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class ProposedProjects : BaseEntity
    {
        public ProposedProjects()
        {
            InvestorsRequests = new HashSet<InvestorsRequests>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProposedProjectId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }

        public int InvestorId { get; set; }

        public virtual Investors Investors { get; set; }

        public virtual ICollection<InvestorsRequests> InvestorsRequests { get; set; }

        //public virtual Projects Projects { get; set; }
    }
}
