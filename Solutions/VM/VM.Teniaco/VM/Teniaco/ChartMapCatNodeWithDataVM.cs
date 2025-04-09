using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    [Serializable]
    public class ChartMapCatNodeWithDataVM 
    {
        public string name { get; set; } = "";

        public string? id { get; set; }
        public string? parent { get; set; }

        public NodeDataVM? data { get; set; }

        public List<ChartMapCatNodeWithDataVM>? children { get; set; } = new List<ChartMapCatNodeWithDataVM>() { };
    }

    public class NodeDataVM
    {
        public string? code { get; set; } = "";

        public int type_id { get; set; } = 0;

        public string type { get; set; } = "";

        public string description { get; set; } = "";

        public long userId { get; set; }

        public long parentUserId { get; set; }

        public int OrgChartNodeId { get; set; }

        public int? ParentOrgChartNodeId { get; set; }

        // public string staffName { get; set; }

        // public List<BoardMembersVM> BoardMembersVMList { get; set; }
    }
}


