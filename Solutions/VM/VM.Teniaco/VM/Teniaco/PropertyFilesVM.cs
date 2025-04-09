using FrameWork;

namespace VM.Teniaco
{
    public class PropertyFilesVM : BaseEntity
    {
        public int PropertyFileId { get; set; }

        public long PropertyId { get; set; }

        public string? PropertyFileTitle { get; set; }

        public string PropertyFilePath { get; set; }

        public string PropertyFileExt { get; set; }

        public int PropertyFileOrder { get; set; }

        public string CreateDate
        {
            set { }
            get
            {
                if (this.CreateEnDate.HasValue)
                    return DateManager.ConvertFromDate("fa", this.CreateEnDate.Value);

                return "";
            }
        }

        public string PropertyFileType { get; set; }//media, docs, maps, ...

        //public virtual PropertiesVM PropertiesVM { get; set; }
    }
}
