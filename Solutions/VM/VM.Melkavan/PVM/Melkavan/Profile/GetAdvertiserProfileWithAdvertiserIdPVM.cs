using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAdvertiserProfileWithAdvertiserIdPVM : BPVM
    {
        public long AdvertiserId { get; set; }
    }
}
