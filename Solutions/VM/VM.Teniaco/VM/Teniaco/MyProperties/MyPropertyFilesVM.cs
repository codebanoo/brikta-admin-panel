using FrameWork;

namespace VM.Teniaco
{
    public class MyPropertyFilesVM : BaseEntity
    {
        public int PropertyFileId { get; set; }

        public long PropertyId { get; set; }

        public string? PropertyFileTitle { get; set; }

        public string PropertyFilePath { get; set; }

        public string PropertyFileExt { get; set; }

        public int PropertyFileOrder { get; set; }

        public string PropertyFileType { get; set; }

        public int? CurrentUserId { get; set; }
    }
}
