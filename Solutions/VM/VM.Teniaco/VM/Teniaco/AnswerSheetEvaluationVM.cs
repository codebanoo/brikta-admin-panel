using FrameWork;

namespace VM.Teniaco
{
    public class AnswerSheetEvaluationVM : BaseEntity
    {
        public virtual List<EvaluationCategoriesVM>? EvaluationCategoriesVMList { get; set; }
        public virtual List<EvaluationQuestionsVM>? EvaluationQuestionsVMList { get; set; }
        public virtual List<EvaluationItemsVM>? EvaluationItemsVMList { get; set; }
        public virtual List<EvaluationItemValuesVM>? EvaluationItemValuesVMList { get; set; }
    }
}
