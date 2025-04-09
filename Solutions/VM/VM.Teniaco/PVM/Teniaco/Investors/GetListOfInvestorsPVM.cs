using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetListOfInvestorsPVM : BPVM
    {
        public long? UserId { get; set; }
        //public long? PersonId { get; set; }
        public string? CompanyName { get; set; }
        public int? FundId { get; set; }
    }
}
