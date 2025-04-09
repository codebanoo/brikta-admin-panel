using FrameWork;

namespace VM.Teniaco
{
    public class CompareFeatureValuesVM : BaseEntity
    {
        public int FeatureId { get; set; }
        public string? FeatureTitle { get; set; }
        public long PropertyId { get; set; }
        public int FeatureValueId { get; set; }
        public string? FeatureValue { get; set; }
        public int? PropertyTypeId { get; set; }

        public int? FeatureOptionId { get; set; }
        public string? FeatureOptionText { get; set; }
        public int? FeatureOptionValue { get; set; }
    }
}
