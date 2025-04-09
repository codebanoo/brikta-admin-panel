using FrameWork;

namespace VM.Teniaco
{
    public class FeaturesOptionsVM : BaseEntity
    {
        public int FeatureOptionId { get; set; }

        public int FeatureId { get; set; }

        public int FeatureOptionValue { get; set; }

        public string FeatureOptionText { get; set; }

        public virtual FeaturesVM? FeaturesVM { get; set; }
    }
}
