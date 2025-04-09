using FrameWork;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace VM.Projects
{
    public class ExcelSheetConfigsVM : BaseEntity
    {
        public long ExcelSheetConfigId { get; set; }
        public string? ExcelSheetConfigName { get; set; }
        public long GoogleSheetConfigId { get; set; }
        public int? GId { get; set; }
        public string SpreadSheetId { get; set; }
        public string? ExcelSheetHour { get; set; }
        public string ReportType { get; set; }//daily, financial, msp
    }
}
