using FrameWork;

namespace VM.Teniaco
{
    public class ComparePropertiesAddressVM : BaseEntity
    {
        public long PropertyId { get; set; }
        public long? StateId { get; set; }
        public string? StateName { get; set; }
        public long? CityId { get; set; }
        public string? CityName { get; set; }
        public long? ZoneId{ get; set; }
        public string? ZoneName { get; set; }
        public long? DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public double? LocationLat { get; set; }
        public double? LocationLon { get; set; }

    }
}
