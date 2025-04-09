using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class EvaluationCategoriesNodeWithDataVM
    {
        public string title { get; set; } = "";
        public bool folder { get; set; } 
        public string? id { get; set; }
        public string? parent { get; set; }
        public string? key { get; set; }
        public bool isScoreOk { get; set; }
        public bool isAnswer { get; set; }

        public List<EvaluationCategoriesNodeWithDataVM>? children { get; set; } = new List<EvaluationCategoriesNodeWithDataVM>() { };

    }
    //public class EvalNodeDataVM
    //{
    //    public string? code { get; set; } = "";

    //    public int type_id { get; set; } = 0;

    //    public string type { get; set; } = "";

    //    public string description { get; set; } = "";

    //}
}
