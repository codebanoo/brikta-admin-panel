using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class FeaturesOptions : BaseEntity
    {
        [Key]
        public int FeatureOptionId { get; set; }

        [ForeignKey("Features")]
        public int FeatureId { get; set; }

        public int FeatureOptionValue { get; set; }

        [Required]
        [StringLength(50)]
        public string FeatureOptionText { get; set; }

        public virtual Features Features { get; set; }
    }
}
