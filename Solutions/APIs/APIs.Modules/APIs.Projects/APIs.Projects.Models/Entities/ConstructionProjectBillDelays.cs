using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class ConstructionProjectBillDelays : BaseEntity
    {
        [Key]
        public int ConstructionProjectBillDelayId { get; set; }
        public string? ConstructionProjectBillDelayTitle { get; set; }
    }
}
