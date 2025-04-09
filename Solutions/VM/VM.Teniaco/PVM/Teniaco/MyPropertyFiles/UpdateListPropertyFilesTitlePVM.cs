using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class UpdateListPropertyFilesTitlePVM : BPVM
    {
        public int PropertyFileId { get; set; }
        public string? PropertyFileTitle { get; set; }
    }
    public class UpdateListPropertyFilesTitlePrm
    {
        public List<UpdateListPropertyFilesTitlePVM> updateListPropertyFilesTitlePVM { get; set; } = new List<UpdateListPropertyFilesTitlePVM>();
    }
}
