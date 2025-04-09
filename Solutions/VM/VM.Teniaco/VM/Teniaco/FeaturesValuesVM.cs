using FrameWork;

namespace VM.Teniaco
{
    public class FeaturesValuesVM : BaseEntity
    {
        public int FeatureValueId { get; set; }

        public int FeatureId { get; set; }

        public long PropertyId { get; set; }

        public string? FeatureValue { get; set; }

        public string? FeatureTitle { get; set; }

        public string? FeatureValueTitle { get; set; }

        public virtual FeaturesVM? FeaturesVM { get; set; }

        public virtual PropertiesVM? PropertiesVM { get; set; }

        //public virtual PropertyTypesVM? PropertyTypesVM { get; set; }
    }
}
