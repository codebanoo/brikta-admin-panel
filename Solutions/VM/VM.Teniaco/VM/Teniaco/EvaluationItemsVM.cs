using FrameWork;

namespace VM.Teniaco
{
    public class EvaluationItemsVM : BaseEntity
    {
        public int EvaluationItemId { get; set; }

        public int EvaluationQuestionId { get; set; }

        //public string EvaluationQuestion { get; set; }

        public string EvaluationAnswer { get; set; }

        public int EvaluationOrder { get; set; }

        public int EvaluationScore { get; set; }
    }
}
