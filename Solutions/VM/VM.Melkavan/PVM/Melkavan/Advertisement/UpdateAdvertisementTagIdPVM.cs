using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.Melkavan.PVM.Melkavan.Advertisement
{
    public class UpdateAdvertisementTagIdPVM : BPVM
    {
        public long AdvertisementId { get; set; }
        public string Type { get; set; }
        public string? TagId { get; set; }
    }
}
