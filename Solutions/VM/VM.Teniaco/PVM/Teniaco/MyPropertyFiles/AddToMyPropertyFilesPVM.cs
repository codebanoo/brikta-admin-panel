using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;
using VM.Teniaco;

namespace VM.PVM.Teniaco
{
    public class AddToMyPropertyFilesPVM : BPVM
    {
        public List<MyPropertyFilesVM> MyPropertyFilesVMList { get; set; }
        public int? CurrentUserId { get; set; }


    }
}
