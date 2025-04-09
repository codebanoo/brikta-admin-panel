using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace APIs.Public.Models.Entities
{
    public partial class DistrictFiles : BaseEntity
    {
        [Key]
        public int DistrictFileId { get; set; }
        [ForeignKey("Districts")]
        public long DistrictId { get; set; }
        public string? DistrictFileTitle { get; set; }
        public string? DistrictFilePath { get; set; }
        public string? DistrictFileExt { get; set; }
        public int? DistrictFileOrder { get; set; }
        public string? DistrictFileType { get; set; }
    }
}
