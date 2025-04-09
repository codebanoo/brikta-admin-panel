using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class OperationOnPropertyProjects : BaseEntity
    {
        [Key]
        public int OperationOnPropertyProjectId { get; set; }

        public int PropertyProjectId { get; set; }

        public int OperationOnPropertyId { get; set; }

        public string OperationOnPropertyProjectDesc { get; set; }
    }
}
