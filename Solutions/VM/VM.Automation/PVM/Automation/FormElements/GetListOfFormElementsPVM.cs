using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Automation
{
    public class GetListOfFormElementsPVM : BPVM
    {
        public int FormId { get; set; }

        public string FormElementTitle { get; set; }
    }
}
