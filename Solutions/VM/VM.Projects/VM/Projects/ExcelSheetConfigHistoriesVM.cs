using FrameWork;

namespace VM.Projects
{
    public class ExcelSheetConfigHistoriesVM : BaseEntity
    {
        public long ExcelSheetConfigHistoryId { get; set; }
        public long ExcelSheetConfigId { get; set; }
        public int? LastReadRow { get; set; }
        public int? CountOfReadRows { get; set; }
        public string? ExcelSheetConfigHistoryFilePath { get; set; }
        public string? ExcelSheetConfigHistoryFileExt { get; set; }
    }
}
