using System.Collections.Generic;
using VM.Melkavan;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class AddToAdvertisementFeaturesValuesPVM : BPVM
    {
        public long AdvertisementId { get; set; }
        public List<AdvertisementFeaturesValuesVM> values { get; set; } = new List<AdvertisementFeaturesValuesVM>();
    }
}
