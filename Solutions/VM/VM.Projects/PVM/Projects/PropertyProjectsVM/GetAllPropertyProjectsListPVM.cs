using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetAllPropertyProjectsListPVM : BPVM
    {
        public long PropertyId { get; set; }

        public int? PropertyProjectTypeId { get; set; }

        public bool? IsPrivate { get; set; }
    }
}
