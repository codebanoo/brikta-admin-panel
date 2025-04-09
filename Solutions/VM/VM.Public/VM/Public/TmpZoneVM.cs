using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Public
{
    public class TmpZoneVM : BaseEntity
    {
        public long? ZoneId { get; set; }

        public long? StateId { get; set; }

        public string? StateName { get; set; }

        public long? CityId { get; set; }

        public string? CityName { get; set; }

        ////نام اختصار
        //public string? Abbreviation { get; set; }

        //public string? VillageName { get; set; }

        public string? ZoneName { get; set; }

        //public string? TownName { get; set; }
    }
}
