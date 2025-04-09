using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIs.Melkavan.Models.Entities
{
    public partial class Banners : BaseEntity
    {
        public int BannerId { get; set; }
        [Required(ErrorMessage = "عنوان را انتخاب کنید")]
        public string BannerTitle { get; set; }
        public string? BannerFilePath { get; set; }
        public string? BannerFileExt { get; set; }
        public string? BannerLink { get; set; }
        public int BannerFileOrder { get; set; }
    }
}
