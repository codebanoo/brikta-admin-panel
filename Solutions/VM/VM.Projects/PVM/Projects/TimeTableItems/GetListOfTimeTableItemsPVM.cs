using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetListOfTimeTableItemsPVM : BPVM
    {
        public long? TimeTableId { get; set; }

        public long? TimeTableItemParentId { get; set; }

        public string TimeTableItemTitle { get; set; }
    }
}
