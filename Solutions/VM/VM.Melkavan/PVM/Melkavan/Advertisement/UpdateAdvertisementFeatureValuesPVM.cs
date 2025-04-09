using System.Collections.Generic;
using VM.Melkavan;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class UpdateAdvertisementFeatureValuesPVM : BPVM
    {
        public List<AdvertisementFeaturesValuesVM> AdvertisementFeaturesValuesVMList { get; set; }

        public long AdvertisementId { get; set; }
    }
}
