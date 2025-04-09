using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAdvertisementViewersWithAdvertisementIdPVM : BPVM
    {
        public int? AdvertisementId { get; set; }
        public string? RecordType { get; set; }
    }
}
