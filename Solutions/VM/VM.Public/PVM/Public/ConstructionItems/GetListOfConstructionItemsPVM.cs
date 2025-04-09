using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetListOfConstructionItemsPVM : BPVM
    {
        public long? ConstructionItemParentId { get; set; }

        public string ConstructionItemTitle { get; set; }

        public DateTime? DateTimeTo { get; set; }
    }
}
