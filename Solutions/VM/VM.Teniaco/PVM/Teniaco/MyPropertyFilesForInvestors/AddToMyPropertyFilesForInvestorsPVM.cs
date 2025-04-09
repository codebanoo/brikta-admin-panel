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
    public class AddToMyPropertyFilesForInvestorsPVM : BPVM
    {
        public List<PropertyFilesVM> PropertyFilesVMList { get; set; }
        public int? CurrentUserId { get; set; }
        public long? PropertyId { get; set; }

    }
}
