using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetListOfPropertyProjectsPVM : BPVM
    {
        public long PropertyId { get; set; }

        public int? PropertyProjectTypeId { get; set; }

        public bool? IsPrivate { get; set; }
    }
}
