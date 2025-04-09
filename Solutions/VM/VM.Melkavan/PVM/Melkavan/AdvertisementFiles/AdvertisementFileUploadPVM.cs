using Microsoft.AspNetCore.Http;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class AdvertisementFileUploadPVM : BPVM
    {
        public int AdvertisementFileId { get; set; }

        public string AdvertisementFileTitle { get; set; }

        public int AdvertisementFileOrder { get; set; }

        public IFormFile File { get; set; }
    }
}
