using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.Projects.TeniacoSuggestions
{
    public class GetListOfTeniacoSuggestedProjectsPVM : BPVM
    {
        public int pageNum { get; set; }
        public int pageSize { get; set; }
    }
}
