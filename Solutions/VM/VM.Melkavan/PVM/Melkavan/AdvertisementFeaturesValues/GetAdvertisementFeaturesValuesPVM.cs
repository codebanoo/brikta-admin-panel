using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAdvertisementFeaturesValuesPVM : BPVM
    {
        public long AdvertisementId { get; set; }

        public int PropertyTypeId { get; set; }
    }
}
