using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Automation
{
    public class BoardMembersVM : BaseEntity
    {
        public int BoardMemberId { get; set; }

        public int OrgChartNodeId { get; set; }

        public long UserId { get; set; }

        public bool IsCeo { get; set; }
    }
}
