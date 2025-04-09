using FrameWork;

namespace VM.Teniaco
{
    public class AgencyLocationVM : BaseEntity
    {
        public int AgencyId { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }
    }
}
