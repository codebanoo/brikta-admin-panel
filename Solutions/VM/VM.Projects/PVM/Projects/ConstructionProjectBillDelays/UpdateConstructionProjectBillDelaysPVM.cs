using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Projects;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class UpdateConstructionProjectBillDelaysPVM : BPVM
    {
        public ConstructionProjectBillDelaysVM? constructionProjectBillDelaysVM { get; set; }
    }
}
