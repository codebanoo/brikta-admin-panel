using FrameWork;
using VM.Public;

namespace VM.Teniaco
{
    public class FeaturesVM : BaseEntity
    {
        public FeaturesVM()
        {
            //ElementTypesVM = new ElementTypesVM();
            FeaturesOptionsVM = new HashSet<FeaturesOptionsVM>();
            FeaturesValuesVM = new HashSet<FeaturesValuesVM>();
        }

        public int FeatureId { get; set; }
        public int FeatureCategoryId { get; set; }
        public string? FeatureCategoryName { get; set; }

        public int PropertyTypeId { get; set; }

        public string FeatureTitle { get; set; }

        public int ElementTypeId { get; set; }

        public bool IsRequired { get; set; }

        public string? DefaultValue { get; set; }

        public virtual ElementTypesVM? ElementTypesVM { get; set; }

        public virtual ICollection<FeaturesOptionsVM> FeaturesOptionsVM { get; set; }

        public virtual ICollection<FeaturesValuesVM> FeaturesValuesVM { get; set; }
    }
}
