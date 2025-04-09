using FrameWork;

namespace VM.Public
{
    public class ZoneFilesVM : BaseEntity
    {
        public int ZoneFileId { get; set; }
        public long ZoneId { get; set; }
        public string? ZoneFileTitle { get; set; }
        public string? ZoneFilePath { get; set; }
        public string? ZoneFileExt { get; set; }
        public int? ZoneFileOrder { get; set; }
        public string? ZoneFileType { get; set; }
    }
}
