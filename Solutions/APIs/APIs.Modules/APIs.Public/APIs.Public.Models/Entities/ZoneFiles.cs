using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace APIs.Public.Models.Entities
{
    public partial class ZoneFiles : BaseEntity
    {
        [Key]
        public int ZoneFileId { get; set; }
        [ForeignKey("Zones")]
        public long ZoneId { get; set; }
        public string? ZoneFileTitle { get; set; }
        public string? ZoneFilePath { get; set; }
        public string? ZoneFileExt { get; set; }
        public int? ZoneFileOrder { get; set; }
        public string? ZoneFileType { get; set; }
    }
}
