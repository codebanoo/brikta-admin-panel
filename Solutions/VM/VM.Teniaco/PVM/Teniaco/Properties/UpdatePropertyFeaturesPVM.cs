using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;
using VM.Teniaco;

namespace VM.PVM.Teniaco
{
    public class UpdatePropertyFeaturesPVM : BPVM
    {
        public List<FeaturesValuesVM> FeaturesValuesVMList { get; set; }

        public long PropertyId { get; set; }
    }
}
