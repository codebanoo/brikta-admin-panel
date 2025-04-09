using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetAllConstructionItemsListPVM : BPVM
    {
        public long? ConstructionItemParentId { get; set; }

        public string ConstructionItemTitle { get; set; }

        public DateTime? DateTimeTo { get; set; }
    }
}
