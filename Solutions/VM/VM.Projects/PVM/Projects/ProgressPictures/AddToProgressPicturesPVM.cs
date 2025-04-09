using Microsoft.AspNetCore.Http;
using VM.Projects;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class AddToProgressPicturesPVM : BPVM
    {
        public ProgressPicturesVM ProgressPicturesVM { get; set; }
        public IFormFile? File { get; set; }
    }
}
