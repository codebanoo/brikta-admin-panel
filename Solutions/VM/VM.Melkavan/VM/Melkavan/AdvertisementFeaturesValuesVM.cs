using FrameWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VM.Public;
using VM.Teniaco;

namespace VM.Melkavan
{
    public class AdvertisementFeaturesValuesVM : BaseEntity
    {
        [Key]
        public int AdvertisementFeatureValueId { get; set; }

        public int FeatureId { get; set; }

        public long AdvertisementId { get; set; }

        public string? FeatureValue { get; set; }


        public AdvertisementFeaturesValuesVM()
        {
            ElementTypesVMList = new List<ElementTypesVM>();
            AdvertisementFeaturesVMList = new List<AdvertisementFeaturesVM>();
            FeaturesOptionsVMList = new List<FeaturesOptionsVM>();
            AdvertisementFeaturesValuesVMList = new List<AdvertisementFeaturesValuesVM>();
            FeaturesVMList = new List<FeaturesVM>();
        }

        public List<ElementTypesVM> ElementTypesVMList { get; set; }

        public List<AdvertisementFeaturesVM> AdvertisementFeaturesVMList { get; set; }

        public List<FeaturesOptionsVM> FeaturesOptionsVMList { get; set; }

        public List<AdvertisementFeaturesValuesVM> AdvertisementFeaturesValuesVMList { get; set; }
        public List<FeaturesCategoriesVM> FeaturesCategoriesVMList {  get; set; }

        public List<FeaturesVM> FeaturesVMList { get; set; }
    }
}
