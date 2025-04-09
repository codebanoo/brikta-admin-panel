using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Projects.Models.Entities
{
    public partial class ExcelSheetConfigs : BaseEntity
    {
        [Key]
        public long ExcelSheetConfigId { get; set; }
        public string? ExcelSheetConfigName { get; set; }
        public long GoogleSheetConfigId { get; set; }
        public int? GId { get; set; }
        public string SpreadSheetId { get; set; }
        public string? ExcelSheetHour { get; set; }
        public string ReportType { get; set; }//daily, financial, msp
    }
}
