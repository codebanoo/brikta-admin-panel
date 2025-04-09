using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public partial class Zones : BaseEntity
    {
        public long ZoneId { get; set; }

        //public long? ParentZoneId { get; set; }

        [ForeignKey("Cities")]
        public long CityId { get; set; }

        //[StringLength(50)]
        //نام اختصار
        //public string Abbreviation { get; set; }

        //[StringLength(50)]
        //public string VillageName { get; set; }

        [StringLength(50)]
        public string ZoneName { get; set; }

        //[StringLength(50)]
        //public string TownName { get; set; }

        public string StrPolygon { get; set; }
        public string? Description { get; set; }
        public Cities Cities { get; set; }
    }
}
