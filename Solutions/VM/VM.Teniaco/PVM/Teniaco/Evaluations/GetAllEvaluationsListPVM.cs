using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetAllEvaluationsListPVM : BPVM
    {
        public int? EvaluationSubjectId { get; set; }
        public string? EvaluationTitle{ get; set; }
    }
}
