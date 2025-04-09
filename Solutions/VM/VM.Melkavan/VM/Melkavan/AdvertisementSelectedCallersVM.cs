using FrameWork;
using System.ComponentModel.DataAnnotations;


namespace VM.Melkavan
{
    public class AdvertisementSelectedCallersVM : BaseEntity
    {
        //key
        public int AdvertisementSelectedCallersId { get; set; }

        //advertisements
        public long AdvertisementId { get; set; }
        public string? CallerType { get; set; }
        public string? AdvertiserNumber { get; set; }
        public string? AgencyNumber { get; set; }
    }
}
