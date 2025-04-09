using FrameWork;
using System.ComponentModel.DataAnnotations;


namespace VM.Melkavan
{
    public class AdvertisementAddressVM : BaseEntity
    {

        public long AdvertisementId { get; set; }

        //[Required(ErrorMessage = "استان را انتخاب کنید")]
        public long? StateId { get; set; }
        public long? TempStateId { get; set; }
        public string? StateName { get; set; }
        public string? TempStateName { get; set; }

        [Required(ErrorMessage = "شهر را انتخاب کنید")]
        public long? CityId { get; set; }
        public long? TempCityId { get; set; }
        public string? CityName { get; set; }

        public string? TempCityName { get; set; }
        //منطقه
        public long? ZoneId { get; set; }
        public long? TempZoneId { get; set; }
        public string? ZoneName { get; set; }
        public string? TempZoneName { get; set; }
        public long? DistrictId { get; set; }
        //public long? TempDistrictId { get; set; }
        public string? DistrictName { get; set; }
        //public string? Abbreviation { get; set; }

        //public string? VillageName { get; set; }

        public string? TownName { get; set; }

        //[Required(ErrorMessage = "آدرس را وارد کنید")]
        public string? Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public virtual AdvertisementVM? AdvertismentsVM { get; set; }
    }
}
