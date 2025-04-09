using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetListOfContractorsPVM : BPVM
    {
        public string? ContractorName { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? ZoneId { get; set; }
    }
}
