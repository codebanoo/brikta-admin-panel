using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetListOfConstructionItemPricesHistoriesPVM : BPVM
    {
        public string ParentType { get; set; }

        public long ParentId { get; set; }

        public DateTime? DateTimeTo { get; set; }
    }
}
