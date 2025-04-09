using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Public
{
    public class ZonesVM : BaseEntity
    {
        public long ZoneId { get; set; }

        public long StateId { get; set; }

        public string StateName { get; set; }

        public long CityId { get; set; }

        public string CityName { get; set; }

        //public long? ParentZoneId { get; set; }

        //نام اختصار
        //public string Abbreviation { get; set; }

        //public string VillageName { get; set; }

        public string ZoneName { get; set; }

        //public string TownName { get; set; }

        public string StrPolygon { get; set; }

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
        public CitiesVM CitiesVM { get; set; }
    }
}
