using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetListOfCitiesPVM : BPVM
    {
        public long? StateId { get; set; }

        public string CityName { get; set; }

        public string CityCode { get; set; }
    }
}
