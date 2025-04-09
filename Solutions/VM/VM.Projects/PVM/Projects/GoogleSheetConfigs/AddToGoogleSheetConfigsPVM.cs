using Microsoft.AspNetCore.Http;
using VM.Projects;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class AddToGoogleSheetConfigsPVM : BPVM
    {
        public GoogleSheetConfigsVM GoogleSheetConfigsVM { get; set; }
    }
}
