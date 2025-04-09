using System;
using System.Collections.Generic;
using System.Text;
using FrameWork;

namespace VM.Public
{
    public class CitiesVM : BaseEntity
    {
        public CitiesVM()
        {
            ZonesVM = new HashSet<ZonesVM>();
        }

        public long CityId { get; set; }

        public long StateId { get; set; }

        public string CityName { get; set; }

        public string CityCode { get; set; }

        public string StrPolygon { get; set; }

        public StatesVM StateVM { get; set; }

        public ICollection<ZonesVM> ZonesVM { get; set; }
    }
}
