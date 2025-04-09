using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Public.Models.Entities
{
    public partial class Districts : BaseEntity
    {
        public long DistrictId { get; set; }

        [StringLength(50)]
        public string? DistrictName { get; set; }

        [ForeignKey("Zone")]
        public long ZoneId { get; set; }

        public string? StrPolygon { get; set; }
        [StringLength(50)]
        public string VillageName { get; set; }
        [StringLength(50)]
        public string TownName { get; set; }
        public string? Description { get; set; }
        //public long?  StateId{ get; set; }
        //public long? CityId { get; set; }
        public Zones Zone { get; set; }
    }
}
