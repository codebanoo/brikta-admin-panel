using FrameWork;

namespace VM.Public
{
    public class DistrictFilesVM : BaseEntity
    {
        public int DistrictFileId { get; set; }
        public long DistrictId { get; set; }
        public string? DistrictFileTitle { get; set; }
        public string? DistrictFilePath { get; set; }
        public string? DistrictFileExt { get; set; }
        public int? DistrictFileOrder { get; set; }
        public string? DistrictFileType { get; set; }
    }
}
