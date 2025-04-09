using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetAllFundsListPVM : BPVM
    {
        public int? FundId { get; set; }

        public string? FundName { get; set; }    

    }
}
