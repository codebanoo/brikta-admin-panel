using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan
{
    public class AdvertisementCallersVM : BaseEntity
    {
        public int AdvertisementCallersId { get; set; }
        public long AdvertisementId { get; set; }
        public string AdvertisementCallerType { get; set; }

    }
}
