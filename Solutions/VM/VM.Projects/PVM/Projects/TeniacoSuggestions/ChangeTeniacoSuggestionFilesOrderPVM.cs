using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.Projects.TeniacoSuggestions
{
    public class ChangeTeniacoSuggestionFilesOrderPVM : BPVM
    {
        public Dictionary<long,int> SuggestionFileOrders { get; set; } = new();
    }
}
