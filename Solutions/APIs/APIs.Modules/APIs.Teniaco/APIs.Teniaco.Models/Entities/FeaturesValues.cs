using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class FeaturesValues : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FeatureValueId { get; set; }

        [ForeignKey("Features")]
        public int FeatureId { get; set; }

        [ForeignKey("Properties")]
        public long PropertyId { get; set; }

        //[Required]
        [StringLength(100)]
        public string? FeatureValue { get; set; }

        public virtual Features Features { get; set; }

        public virtual Properties Properties { get; set; }

        //public virtual PropertyTypes PropertyTypes { get; set; }
    }
}
