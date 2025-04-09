using Microsoft.AspNetCore.Http;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class ZoneFileUploadPVM : BPVM
    {
        public int ZoneFileId { get; set; }

        public string ZoneFileTitle { get; set; }

        public int ZoneFileOrder { get; set; }

        public IFormFile File { get; set; }
    }
}
