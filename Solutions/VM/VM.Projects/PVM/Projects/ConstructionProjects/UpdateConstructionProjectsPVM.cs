using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;
using VM.Projects;

namespace VM.PVM.Projects
{
    public class UpdateConstructionProjectsPVM : BPVM
    {
        public ConstructionProjectsVM? ConstructionProjectsVM { get; set; }
        public long? PersonId { get; set; }
        public int? ContractSideTypeId { get; set; }
    }
}
