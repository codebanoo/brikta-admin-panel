using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Melkavan.Models.Entities
{
    public class StatusTypes : BaseEntity
    {
        [Key]
        public int StatusId { get; set; }
        public string? StatusTitle { get; set; }
    }
}
