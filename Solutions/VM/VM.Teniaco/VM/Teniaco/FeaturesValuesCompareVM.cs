using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class FeaturesValuesCompareVM : BaseEntity
    {
        public int FeatureValueId { get; set; }
        public int FeatureId { get; set; }
        public long PropertyId { get; set; }
        public string? FeatureValue { get; set; }

        public string? FeatureTitle { get; set; }
    }
}
