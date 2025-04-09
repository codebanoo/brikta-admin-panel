using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan
{
    public class AdvertisementLocation : BaseEntity
    {
        public long AdvertisementId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int? ZoneId { get; set; }
        public int? DistrictId { get; set; }
        public string? Address { get; set; }
        public double? LocationLat { get; set; }
        public double? LocationLon { get; set; }
        public string? TownName { get; set; }
    }
}
