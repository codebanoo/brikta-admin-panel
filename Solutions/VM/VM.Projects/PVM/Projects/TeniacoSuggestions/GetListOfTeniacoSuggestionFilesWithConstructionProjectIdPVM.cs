using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace PVM.Projects.TeniacoSuggestions
{
    public class GetListOfTeniacoSuggestionFilesWithConstructionProjectIdPVM : BPVM
    {
        public long ConstructionProjectId {  get; set; }
    }
}
