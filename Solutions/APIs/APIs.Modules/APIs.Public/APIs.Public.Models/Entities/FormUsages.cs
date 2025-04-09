using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public partial class FormUsages : BaseEntity
    {
        [Key]
        public int FormUsageId { get; set; }

        public string FormUsageTitle { get; set; }
    }
}
