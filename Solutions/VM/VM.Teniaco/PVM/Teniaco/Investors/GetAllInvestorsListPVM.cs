
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetAllInvestorsListPVM : BPVM
    {
        public long? UserId { get; set; }
        //public long? PersonId { get; set; }
        public string? CompanyName { get; set; }
        public int? FundId { get; set; }
    }
}
