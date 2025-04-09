using FrameWork;

namespace VM.Teniaco
{
    public class EvaluationCategoriesVM : BaseEntity
    {
        public int EvaluationCategoryId { get; set; }

        public int? EvaluationParentCategoryId { get; set; }

        public string EvaluationCategoryTitle { get; set; }

        public int EvaluationCategoryOrder { get; set; }

        public double EvaluationCategoryScore { get; set; }
        public int EvaluationId { get; set; }

        public int? CountOfEvaluationQuestions { get; set; }
    }
}
