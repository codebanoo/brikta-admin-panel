using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementFeaturesValues : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvertisementFeatureValueId { get; set; }

        //[ForeignKey("Features")]
        public int FeatureId { get; set; }

        [ForeignKey("Advertisement")]
        public long AdvertisementId { get; set; }

        //[Required]
        [StringLength(100)]
        public string? FeatureValue { get; set; }

        //public virtual Features Features { get; set; }

        public virtual Advertisement Advertisment { get; set; }

        //public virtual PropertyTypes PropertyTypes { get; set; }
    }
}
