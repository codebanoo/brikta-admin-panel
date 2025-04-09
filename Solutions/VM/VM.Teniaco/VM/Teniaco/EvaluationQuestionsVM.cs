using FrameWork;

namespace VM.Teniaco
{
    public class EvaluationQuestionsVM : BaseEntity
    {
        public int? EvaluationQuestionId { get; set; }

        public int? EvaluationCategoryId { get; set; }

        //public string EvaluationCategoryTitle { get; set; }

        public string? EvaluationQuestion { get; set; }

        public int? EvaluationOrder { get; set; }

        public int? EvaluationScore { get; set; }

        public int? CountOfEvaluationItems { get; set; }

        //public int? RootId { get; set; }
    }
}
