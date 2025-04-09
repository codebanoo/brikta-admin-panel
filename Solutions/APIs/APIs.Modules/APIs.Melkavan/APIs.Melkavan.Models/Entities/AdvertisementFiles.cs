using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementFiles : BaseEntity
    {
        [Key]
        public int AdvertisementFileId { get; set; }

        [ForeignKey("Advertisement")]
        public long AdvertisementId { get; set; }

        //[Required]
        //[StringLength(50)]
        public string? AdvertisementFileTitle { get; set; }

        [Required]
        [StringLength(200)]
        public string AdvertisementFilePath { get; set; }

        [Required]
        [StringLength(5)]
        public string AdvertisementFileExt { get; set; }

        public string AdvertisementFileType { get; set; }

        public int AdvertisementFileOrder { get; set; }

        public virtual Advertisement Advertisement { get; set; }
    }
}
