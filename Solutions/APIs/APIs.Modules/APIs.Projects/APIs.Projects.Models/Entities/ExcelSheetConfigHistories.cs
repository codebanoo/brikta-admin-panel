using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Projects.Models.Entities
{
    public partial class ExcelSheetConfigHistories : BaseEntity
    {
        [Key]
        public long ExcelSheetConfigHistoryId { get; set; }
        public long ExcelSheetConfigId { get; set; }
        public int? LastReadRow { get; set; }
        public int? CountOfReadRows { get; set; }
        public string? ExcelSheetConfigHistoryFilePath { get; set; }
        public string? ExcelSheetConfigHistoryFileExt { get; set; }
    }
}
