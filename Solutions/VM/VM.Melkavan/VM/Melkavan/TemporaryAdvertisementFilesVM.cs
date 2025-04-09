using FrameWork;
using Microsoft.AspNetCore.Http;


namespace VM.Melkavan
{
    public class TemporaryAdvertisementFilesVM : BaseEntity
    {
        public string AdvertisementFilePath { get; set; }

        public IFormFile MyFile { get; set; }
    }
}
