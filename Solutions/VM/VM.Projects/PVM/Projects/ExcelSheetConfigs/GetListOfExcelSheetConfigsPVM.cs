using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetListOfExcelSheetConfigsPVM : BPVM
    {
        public string? ExcelSheetConfigName { get; set; }
        public long? GoogleSheetConfigId { get; set; }
    }
}
