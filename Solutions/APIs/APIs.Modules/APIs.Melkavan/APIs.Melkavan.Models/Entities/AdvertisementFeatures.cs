using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Melkavan.Models.Entities
{
    public class AdvertisementFeatures : BaseEntity
    {
        [Key]
        public int AdvertisementFeaturesId { get; set; }

        public int FeatureId { get; set; }
      
        public int AdvertisementTypeId { get; set; }

        public int? AdvertisementFeatureOrder { get; set; }

        // public int? PropertyTypeId { get; set; }

        //[Required]
        //[StringLength(50)]
        //public string? FeatureTitle { get; set; }

        //[ForeignKey("ElementTypes")]
        //public int? ElementTypeId { get; set; }

        //public bool? IsRequired { get; set; }

        //public string? DefaultValue { get; set; }

        //public virtual ElementTypes ElementTypes { get; set; }

        //public virtual ICollection<FeaturesOptions> FeaturesOptions { get; set; }

        //public virtual ICollection<AdvertisementFeaturesValues> AdvertisementFeaturesValues { get; set; }

        public AdvertisementFeatures()
        {
            //FeaturesOptions = new HashSet<FeaturesOptions>();
            //AdvertisementFeaturesValues = new HashSet<AdvertisementFeaturesValues>();
        }
    }
}
