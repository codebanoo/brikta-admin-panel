using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Public;
using VM.Teniaco;

namespace VM.Melkavan
{
    public class AdvertisementFeaturesVM : BaseEntity
    {
        public int AdvertisementFeaturesId { get; set; }
        public int FeatureId { get; set; }
        public int AdvertisementTypeId { get; set; }
        public int? AdvertisementFeatureOrder { get; set; }


        //public int? PropertyTypeId { get; set; }

        // public string? FeatureTitle { get; set; }

        //public int? ElementTypeId { get; set; }

        //public bool? IsRequired { get; set; }

        //public string? DefaultValue { get; set; }

        // public virtual ElementTypesVM? ElementTypesVM { get; set; }

        public virtual ICollection<FeaturesOptionsVM> FeaturesOptionsVM { get; set; }

        public virtual ICollection<AdvertisementFeaturesValuesVM> AdvertisementFeaturesValuesVM { get; set; }
    }
}
