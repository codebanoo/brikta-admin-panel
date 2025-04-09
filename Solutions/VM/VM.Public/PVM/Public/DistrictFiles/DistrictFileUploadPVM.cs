using Microsoft.AspNetCore.Http;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class DistrictFileUploadPVM : BPVM
    {
        public int DistrictFileId { get; set; }

        public string DistrictFileTitle { get; set; }

        public int DistrictFileOrder { get; set; }

        public IFormFile File { get; set; }
    }
}
