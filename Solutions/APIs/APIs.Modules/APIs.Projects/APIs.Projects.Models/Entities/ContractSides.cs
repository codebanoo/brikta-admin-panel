using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class ContractSides : BaseEntity
    {
        [Key]
        public long ContractSideId { get; set; }
        public int? ContractSideTypeId { get; set; }
        public long? TableRecordId { get; set; }
        public string? TableTitle { get; set; }
        public long? ParentId { get; set; }

    }
}
