using FrameWork;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace VM.Melkavan
{
    public class AdvertisementFilesVM : BaseEntity
    {
        public int AdvertisementFileId { get; set; }

        public long AdvertisementId { get; set; }

        public string? AdvertisementFileTitle { get; set; }

        public string AdvertisementFilePath { get; set; }

        public string AdvertisementFileExt { get; set; }

        public int AdvertisementFileOrder { get; set; }

        public string AdvertisementFileType { get; set; }//media, docs, maps, ...

        //public IFormFile? File { get; set; }

    }
}
