using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Automation;
using VM.PVM.Base;

namespace VM.PVM.Automation
{
    public class EditBoardMembersPVM : BPVM
    {
        public List<BoardMembersVM> BoardMembersVMList { get; set; }

        public int OrgChartNodeId { get; set; }
    }
}
