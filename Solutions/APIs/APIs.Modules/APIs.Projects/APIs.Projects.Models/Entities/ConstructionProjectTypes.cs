using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectTypes : BaseEntity
    {
        [Key]
        public int ConstructionProjectTypeId { get; set; }
        public string ConstructionProjectTypeTitle { get; set; }

    }
}
