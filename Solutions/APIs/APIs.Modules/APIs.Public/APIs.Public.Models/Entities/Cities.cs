using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIs.Public.Models.Entities
{
    public partial class Cities : BaseEntity
    {
        public Cities()
        {
            Zones = new HashSet<Zones>();
        }

        public long CityId { get; set; }

        [ForeignKey("State")]
        public long? StateId { get; set; }

        [StringLength(50)]
        public string CityName { get; set; }

        [StringLength(10)]
        public string CityCode { get; set; }

        public string StrPolygon { get; set; }

        public States State { get; set; }
        public ICollection<Zones> Zones { get; set; }
    }
}
