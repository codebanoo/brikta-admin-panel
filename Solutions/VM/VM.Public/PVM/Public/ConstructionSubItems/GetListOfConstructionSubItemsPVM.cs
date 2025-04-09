using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetListOfConstructionSubItemsPVM : BPVM
    {
        public long? ConstructionItemId { get; set; }

        public string ConstructionSubItemTitle { get; set; }
    }
}
