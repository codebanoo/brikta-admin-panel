using FrameWork;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class ConstructionProjectDelays : BaseEntity
    {
        [Key]
        public long ConstructionProjectsDelayId { get; set; }
        [Required]
        public long ConstructionProjectId { get; set; }
        [Required]
        public string ConstructionProjectsDelayDates { get; set; }
        [Required]
        public int ConstructionProjectBillDelayId { get; set; }
    }
}
