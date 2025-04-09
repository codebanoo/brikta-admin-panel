using FrameWork;


namespace VM.Public
{
    public class DistrictsVM : BaseEntity
    {
        public long DistrictId { get; set; }
        public string? DistrictName { get; set; }
        public long ZoneId { get; set; }
        public string? StrPolygon { get; set; }
        public long? StateId { get; set; }
        public string? StateName { get; set; }
        public long? CityId { get; set; }
        public string? CityName { get; set; }
        public string? ZoneName { get; set; }
        public string? VillageName { get; set; }
        public string? TownName { get; set; }
        public string? Description { get; set; }
        public string? NewDescription
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.Description)))
                {
                    return this.Description.Substring(0, 50);
                }
                else { return this.Description; }
            }
        }

        public int? CountOfMaps { get; set; }
        public int? CountOfDocs { get; set; }
        public int? CountOfMedia { get; set; }

    }
}
