using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetConstructionProjectFinancialDataByConstructionProjectIdPVM : BPVM
    {
        public long ConstructionProjectId { get; set; }
        public string Type { get; set; }
        
        //سهامدار
        public long? OwnerUserId { get; set; }
        
    }
}
