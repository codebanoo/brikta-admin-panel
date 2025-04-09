using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class MyPropertiesCompareBasicInfoVM : BaseEntity
    {
        public string? PropertyCodeName { get; set; }
        public string? PropertyTypesName { get; set; } 
        public string? TypeOfUses { get; set; }

        public string? DocumentTypes { get; set; }
        public string? Area { get; set; } 
        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public string? ZoneName { get; set; }
        public string? DistrictName { get; set; }

        public double? LocationLat { get; set; }
        public double? LocationLon { get; set; }



    }
   
}
