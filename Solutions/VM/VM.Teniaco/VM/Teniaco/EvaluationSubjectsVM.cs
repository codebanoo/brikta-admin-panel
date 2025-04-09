using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class EvaluationSubjectsVM : BaseEntity
    {
        public int EvaluationSubjectId { get; set; }

        public string Title { get; set; } = "title";
    }
}
