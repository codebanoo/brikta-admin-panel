using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetListOfMyPropertyFilesPVM : BPVM
    {
        public long PropertyId { get; set; }

        public string PropertyFileTitle { get; set; }

        public string PropertyFileType { get; set; }
    }
}
