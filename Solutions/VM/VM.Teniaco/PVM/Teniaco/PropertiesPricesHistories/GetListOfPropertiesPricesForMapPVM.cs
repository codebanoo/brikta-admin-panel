using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetListOfPropertiesPricesForMapPVM : BPVM
    {
        public int Platform { get; set; }//all, teniaco, melkavan

        public long? PriceFrom { get; set; }

        public long? PriceTo { get; set; }

        public int? StateId { get; set; }

        public int? CityId { get; set; }

        public int? ZoneId { get; set; }

        public int? DistrictId { get; set; }

        public int? TypeOfUseId { get; set; }

        public int? PropertyTypeId { get; set; }
    }
}
