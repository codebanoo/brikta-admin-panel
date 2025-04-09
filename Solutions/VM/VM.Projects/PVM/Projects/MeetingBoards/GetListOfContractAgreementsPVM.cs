using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetListOfMeetingBoardsPVM : BPVM
    {
        public long? ConstructionProjectId { get; set; }
        public string? MeetingBoardTitle { get; set; }
    }
}
