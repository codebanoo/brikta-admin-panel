using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class EvaluationsVM : BaseEntity
    {
        public int EvaluationId { get; set; }
        public int EvaluationSubjectId { get; set; }
        public DateTime PlanDate { get; set; }
        public string PPlanDate
        {
            get
            {
                return DateManager.ConvertFromDate("fa", this.PlanDate);
            }
            set
            {
                this.PlanDate = DateManager.ConvertToDate("fa", value).Value;
            }
        }
        public string Version { get; set; }
        //public int? QuestionCount { get; set; }
        public string? SubjectsTitle { get; set; }
        public string EvaluationTitle { get; set; }
    }
}
