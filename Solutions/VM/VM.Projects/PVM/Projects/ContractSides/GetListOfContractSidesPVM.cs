using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetListOfContractSidesPVM : BPVM
    {
        public long? ParentId { get; set; }
        public long? TableId { get; set; }
        public string? TableTitle { get; set; }
    }
}
