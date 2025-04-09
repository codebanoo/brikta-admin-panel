using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Teniaco.Models.Entities
{
    public class EvaluationItemValues : BaseEntity
    {
        [Key]
        public int EvaluationItemValueId { get; set; }
        public long ParentId { get; set; }
        public string ParentType { get; set; }
        public int EvaluationQuestionId { get; set; }
        public int? EvaluationItemId { get; set; }

    }
}
