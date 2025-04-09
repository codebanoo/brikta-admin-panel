using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class GetListOfEvaluationCategoriesPVM : BPVM
    {
        public int? EvaluationParentCategoryId { get; set; }

        public string EvaluationCategoryTitleSearch { get; set; }
        public int EvaluationId { get; set; }
    }
}
