using Microsoft.AspNetCore.Http;
using VM.Projects;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class AddToExcelSheetConfigsPVM : BPVM
    {
        public ExcelSheetConfigsVM ExcelSheetConfigsVM { get; set; }
    }
}
