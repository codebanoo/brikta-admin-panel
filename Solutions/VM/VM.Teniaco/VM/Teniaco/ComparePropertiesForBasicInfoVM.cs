using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class ComparePropertiesForBasicInfoVM : BaseEntity
    {

        public long? PropertyId { get; set; }
        public string? PropertyCodeName { get; set; }
        public int? PropertyTypeId { get; set; }
        public string? PropertyTypeTitle{ get; set; }
        public int? TypeOfUseId { get; set; }
        public string? TypeOfUseTitle { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentTypeTitle{ get; set; }
        public string? Area { get; set; }
        

        //public string? StateName { get; set; }
        //public string? CityName { get; set; }
        //public string? ZoneName { get; set; }
        //public string? DistrictName { get; set; }
        //public double? LocationLat { get; set; }
        //public double? LocationLon { get; set; }



    }
   
}
