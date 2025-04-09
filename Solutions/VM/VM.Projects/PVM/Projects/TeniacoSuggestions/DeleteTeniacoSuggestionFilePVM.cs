using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace PVM.Projects.TeniacoSuggestions
{
    public class DeleteTeniacoSuggestionFilePVM : BPVM
    {
        public long SuggestionFileId { get; set; }
    }
}
