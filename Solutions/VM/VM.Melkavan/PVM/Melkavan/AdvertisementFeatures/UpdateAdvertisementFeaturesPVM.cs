using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Melkavan;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class UpdateAdvertisementFeaturesPVM : BPVM
    {
        public int PropertyTypeId { get; set; }

        public List<AdvertisementFeaturesVM> AdvertisementFeaturesVMs { get; set; }
    }
}
