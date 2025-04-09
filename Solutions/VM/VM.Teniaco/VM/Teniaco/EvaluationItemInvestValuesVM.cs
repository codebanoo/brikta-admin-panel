using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class EvaluationItemInvestValuesVM : BaseEntity
    {
        public int EvaluationItemInvestValueId { get; set; } = 0;

        public int InvestorId { get; set; } = 0;
        public int EvaluationQuestionId { get; set; } = 0;
        public int EvaluationItemId { get; set; } = 0;

    }
}
