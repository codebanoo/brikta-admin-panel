using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetAllAgenciesListPVM : BPVM
    {
       // public int? AgencyId { get; set; }

        public string? AgencyName { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? ZoneId { get; set; }
    }
}
