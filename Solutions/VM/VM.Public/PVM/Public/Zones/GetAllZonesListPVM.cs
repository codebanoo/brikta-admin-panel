using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetAllZonesListPVM : BPVM
    {
        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public string SearchTitle { get; set; }
    }
}
