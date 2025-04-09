using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetAllFeaturesListPVM : BPVM
    {
        public int? PropertyTypeId { get; set; }

        public string FeatureTitleSearch { get; set; }
    }
}
