using System.Collections.Generic;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class UpdateListAdvertisementFilesTitlePVM : BPVM
    {
        public int AdvertisementFileId { get; set; }
        public string? AdvertisementFileTitle { get; set; }
    }
    public class UpdateListAdvertisementFilesTitlePrm
    {
        public List<UpdateListAdvertisementFilesTitlePVM> updateListAdvertisementFilesTitlePVM { get; set; } = new List<UpdateListAdvertisementFilesTitlePVM>();
    }
}
