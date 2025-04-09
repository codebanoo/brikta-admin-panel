using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Automation.Models.Entities
{
    public partial class NodeTypes
    {
        public NodeTypes()
        {
        }

        public int NodeTypeId { get; set; }

        public string NodeTypeTitle { get; set; }

        public bool IsShow { get; set; }
    }
}
