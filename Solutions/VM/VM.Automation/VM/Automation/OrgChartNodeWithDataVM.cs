using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Automation
{
    public class OrgChartNodeWithDataVM
    {
        public List<long> UserIds { get; set; }

        public string name { get; set; }

        public string id { get; set; }

        public NodeDataVM data { get; set; }

        public List<OrgChartNodeWithDataVM> children { get; set; }
    }

    public class NodeDataVM
    { 
        public string code { get; set; }

        public int type_id { get; set; }

        public string type { get; set; }

        public string description { get; set; }

        public long userId { get; set; }

        public long parentUserId { get; set; }

        public int OrgChartNodeId { get; set; }

        public int? ParentOrgChartNodeId { get; set; }

        public string staffName { get; set; }

        public List<BoardMembersVM> BoardMembersVMList { get; set; }
    }
}
