using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Automation.Models.Entities
{
    [NotMapped]
    public class OrgChartNodeIds
    {
        [Key]
        //public long Key { get; set; }
        public int OrgChartNodeId { get; set; }
    }
}
