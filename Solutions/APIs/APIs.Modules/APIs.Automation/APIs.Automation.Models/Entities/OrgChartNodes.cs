using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Automation.Models.Entities
{
    public partial class OrgChartNodes : BaseEntity
    {
        public OrgChartNodes()
        {
        }

        public int OrgChartNodeId { get; set; }

        //1 = ساختار سازمانی
        //2 = هیئت مدیره
        //4 = گروه
        //5 = شرکت
        //6 = معاونت
        //7 = واحد سازمانی
        //8 = شخص
        public int NodeTypeId { get; set; }

        public string NodeTitle { get; set; }

        public string NodeDescription { get; set; }

        public int? ParentOrgChartNodeId { get; set; }

        //public long? RelatedId { get; set; }
    }
}
