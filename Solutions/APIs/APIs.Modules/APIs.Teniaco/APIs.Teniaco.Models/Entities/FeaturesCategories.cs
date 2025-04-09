using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Teniaco.Models.Entities
{
    public class FeaturesCategories : BaseEntity
    {
        [Key]
        public int FeatureCategoryId {  get; set; }
        [StringLength(50)]
        [Required]
        public string FeatureCategoryTitle { get; set; }
    }
}
