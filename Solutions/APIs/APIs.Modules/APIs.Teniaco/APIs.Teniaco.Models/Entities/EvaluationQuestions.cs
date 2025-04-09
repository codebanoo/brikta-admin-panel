using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class EvaluationQuestions : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationQuestionId { get; set; }

        public int? EvaluationCategoryId { get; set; }

        //public string EvaluationCategoryTitle { get; set; }

        public string EvaluationQuestion { get; set; }

        public int EvaluationOrder { get; set; }

        public int EvaluationScore { get; set; }

        // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //public int? RootId { get; set; }
    }
}
