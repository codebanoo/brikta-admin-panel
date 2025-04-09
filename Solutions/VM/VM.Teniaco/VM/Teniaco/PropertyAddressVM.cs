using FrameWork;
using System.ComponentModel.DataAnnotations;
using VM.Teniaco;

namespace VM.Teniaco
{
    public class PropertyAddressVM : BaseEntity
    {
        public long PropertyId { get; set; }

        [Required(ErrorMessage = "استان را انتخاب کنید")]
        public long? StateId { get; set; }

        public string? StateName { get; set; }

        [Required(ErrorMessage = "شهر را انتخاب کنید")]
        public long? CityId { get; set; }

        public string? CityName { get; set; }

        //منطقه
        public long? ZoneId { get; set; }

        public string? ZoneName { get; set; }

        public long? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        //public string? Abbreviation { get; set; }

        public string? VillageName { get; set; }

        public string? TownName { get; set; }

        [Required(ErrorMessage = "آدرس را وارد کنید")]
        public string? Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public virtual PropertiesVM? PropertiesVM { get; set; }
    }
}
