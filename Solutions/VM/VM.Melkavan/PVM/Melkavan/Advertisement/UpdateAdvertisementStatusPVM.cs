using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.Melkavan.PVM.Melkavan.Advertisement
{
    public class UpdateAdvertisementStatusPVM : BPVM
    {
        public long AdvertisementId { get; set; }
        public string Type { get; set; }
        public int? StatusId {  get; set; }
        public string? RejectionReason { get; set; }
    }
}
