using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetListOfDistrictsPVM : BPVM
    {
        public long? ZoneId { get; set; }
        public string? DistrictName { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
    }
}
