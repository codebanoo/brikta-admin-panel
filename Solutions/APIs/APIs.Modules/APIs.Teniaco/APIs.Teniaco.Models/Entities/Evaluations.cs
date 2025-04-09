using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public class Evaluations : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EvaluationId { get; set; }
        public int EvaluationSubjectId { get; set; }
        public DateTime PlanDate { get; set; }
        public string Version { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? QuestionCount { get; }
        public string EvaluationTitle { get; set; }


    }
}
