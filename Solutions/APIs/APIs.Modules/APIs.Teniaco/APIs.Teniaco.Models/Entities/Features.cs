using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class Features : BaseEntity
    {
        public Features()
        {
            FeaturesOptions = new HashSet<FeaturesOptions>();
            FeaturesValues = new HashSet<FeaturesValues>();
        }

        [Key]
        public int FeatureId { get; set; }

        public int PropertyTypeId { get; set; }
        [Required]
        public int FeatureCategoryId {  get; set; }

        [Required]
        [StringLength(50)]
        public string FeatureTitle { get; set; }

        [ForeignKey("ElementTypes")]
        public int ElementTypeId { get; set; }

        public bool IsRequired { get; set; }

        public string? DefaultValue { get; set; }

        public virtual ElementTypes ElementTypes { get; set; }

        public virtual ICollection<FeaturesOptions> FeaturesOptions { get; set; }

        public virtual ICollection<FeaturesValues> FeaturesValues { get; set; }
    }
}
