using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class InvestorsFavorites : BaseEntity
    {
        [Key]
        public int InvestorFavoiteId { get; set; }

        [ForeignKey("Properties")]
        public long PropertyId { get; set; }

        [ForeignKey("Investors")]
        public int InvestorId { get; set; }

        public virtual Investors Investors { get; set; }

        public virtual Properties Properties { get; set; }
    }
}
