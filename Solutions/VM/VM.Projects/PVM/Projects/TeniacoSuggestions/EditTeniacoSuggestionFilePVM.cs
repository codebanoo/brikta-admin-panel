using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.Projects.TeniacoSuggestions
{
    public class EditTeniacoSuggestionFilePVM : BPVM
    {
        public List<TeniacoSuggestionFilesVM> teniacoSuggestionFilesVM {  get; set; }
    }
}
