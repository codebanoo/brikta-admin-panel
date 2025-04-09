using VM.PVM.Base;
using VM.Teniaco;

namespace VM.PVM.Teniaco
{
    public class UpdateEvaluationItemValuesPVM : BPVM
    {
        public EvaluationItemValuesVM? EvaluationItemValuesVM { get; set; }
        public long ParentId { get; set; }
        public string replies { get; set; }
    }
}
