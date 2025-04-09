using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class EvaluationItems : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationItemId { get; set; }

        public int? EvaluationQuestionId { get; set; }

        public string EvaluationAnswer { get; set; }

        public int EvaluationOrder { get; set; }

        public int EvaluationScore { get; set; }
    }
}
