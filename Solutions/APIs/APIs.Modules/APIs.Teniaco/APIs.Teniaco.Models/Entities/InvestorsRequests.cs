using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class InvestorsRequests : BaseEntity
    {
        [Key]
        public int InvestorRequestId { get; set; }

        public int InvestorId { get; set; }

        public int ProjectId { get; set; }

        public int? ProposedProjectId { get; set; }

        public virtual Investors Investors { get; set; }

        //public virtual Projects Projects { get; set; }

        public virtual ProposedProjects ProposedProjects { get; set; }
    }
}
