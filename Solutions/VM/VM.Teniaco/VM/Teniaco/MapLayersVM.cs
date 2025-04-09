using FrameWork;

namespace VM.Teniaco
{
    public class MapLayersVM : BaseEntity
    {
        public int MapLayerId { get; set; }
        public int MapLayerCategoryId { get; set; }
        public string? StrPolygon { get; set; }

        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? ZoneId { get; set; }
        public int? DistrictId { get; set; }
    }
}
