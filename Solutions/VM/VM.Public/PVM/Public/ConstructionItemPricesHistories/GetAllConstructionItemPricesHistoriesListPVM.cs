using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetAllConstructionItemPricesHistoriesListPVM : BPVM
    {
        public string ParentType { get; set; }

        public long ParentId { get; set; }
    }
}
