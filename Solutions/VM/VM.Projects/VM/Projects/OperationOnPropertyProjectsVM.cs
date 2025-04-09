using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class OperationOnPropertyProjectsVM : BaseEntity
    {
        public int OperationOnPropertyProjectId { get; set; }

        public int PropertyProjectId { get; set; }

        public int OperationOnPropertyId { get; set; }

        public string OperationOnPropertyProjectDesc { get; set; }
    }
}
