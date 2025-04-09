using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Teniaco;

namespace VM.Melkavan
{
    public class ManageAdvertisementFeaturesValuesVM : BaseEntity
    {
        public List<FeaturesVM> FeaturesVMList { get; set; }
        public List<AdvertisementFeaturesVM> AdvertisementFeaturesVMList { get; set; }
    }
}
