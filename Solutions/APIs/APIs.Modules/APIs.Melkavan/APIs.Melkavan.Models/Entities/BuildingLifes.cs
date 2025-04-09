using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Melkavan.Models.Entities
{
    public class BuildingLifes : BaseEntity
    {
        [Key]
        public int BuildingLifeId { get; set; }
        public string BuildingLifeTitle { get; set; }
    }
}
