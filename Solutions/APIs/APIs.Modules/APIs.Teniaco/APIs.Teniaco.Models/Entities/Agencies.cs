using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Teniaco.Models.Entities
{
    public partial class Agencies : BaseEntity
    {
        [Key]
        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public string? Telephone { get; set; }

        public long StateId { get; set; }

        public long CityId { get; set; }

        public long? ZoneId { get; set; }

        public string? Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public string? Site { get; set; }

        public string? SocialNetworks { get; set; }

        public List<AgencyStaffs> AgencyStaffs { get; set; }
    }
}
