using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetAllDivisionOfEvaluationsListByParentIdPVM : BPVM
    {
        public long? ParentId { get; set; }
        public string? ParentType { get; set; }
        public int? EvaluationId { get; set; }
    }
}
