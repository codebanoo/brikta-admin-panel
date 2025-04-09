using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetListOfConstructionProjectDelaysPVM : BPVM
    {
        public long ConstructionProjectId { get; set; }

        public int? ConstructionProjectBillDelayId { get; set; }
    }
}
