using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class FeaturesCategoriesVM : BaseEntity
    {
        public int FeatureCategoryId { get; set; }
        public string FeatureCategoryTitle { get; set; }
    }
}
