using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class OperationsOnProperty : BaseEntity
    {
        [Key]
        public int OperationOnPropertyId { get; set; }

        [Required]
        [StringLength(50)]
        public string OperationOnPropertyTitle { get; set; }
    }
}
