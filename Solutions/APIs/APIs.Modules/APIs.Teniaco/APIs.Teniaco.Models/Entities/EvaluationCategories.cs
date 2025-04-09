using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Entities
{
    public partial class EvaluationCategories : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationCategoryId { get; set; }

        public int? EvaluationParentCategoryId { get; set; }

        public string EvaluationCategoryTitle { get; set; }

        public int? EvaluationCategoryOrder { get; set; }

        public double? EvaluationCategoryScore { get; set; }
        public int EvaluationId { get; set; }
    }
}
