using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class UploadGeographicDataPVM : BPVM
    {
        public int MapLayerCategoryId { get; set; }
        public IFormFile? File { get; set; }
        public int? CityId { get; set; }
        public int? ZoneId { get; set; }
        public int? DistrictId { get; set; }
    }
}
