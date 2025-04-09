using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ContractSidesVM : BaseEntity
    {
        public long ContractSideId { get; set; }
        public int? ContractSideTypeId { get; set; }
        public long? TableRecordId { get; set; }
        public string? TableTitle { get; set; }
        public long? ParentId { get; set; }

    }
}
