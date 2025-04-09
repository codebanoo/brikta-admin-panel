using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan
{
    public class BannersVM : BaseEntity
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
