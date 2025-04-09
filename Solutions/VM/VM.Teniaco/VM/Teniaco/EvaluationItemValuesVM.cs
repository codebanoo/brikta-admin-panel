using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace VM.Teniaco
{
    public class EvaluationItemValuesVM : BaseEntity
    {
        public int EvaluationItemValueId { get; set; }
        public long ParentId { get; set; }
        public string ParentType { get; set; }
        public int EvaluationQuestionId { get; set; }
        public int? EvaluationItemId { get; set; }

    }
}
