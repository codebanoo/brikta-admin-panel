using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class ConfirmationTypes : BaseEntity
    {
        [Key]
        public int ConfirmationTypeId { get; set; }
        public string? ConfirmationTypeTitle { get; set; }
    }
}
