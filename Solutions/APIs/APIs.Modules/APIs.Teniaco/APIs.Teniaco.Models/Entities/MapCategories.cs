using FrameWork;
using System.ComponentModel.DataAnnotations;



namespace APIs.Teniaco.Models.Entities
{
    public class MapCategories : BaseEntity
    {
        [Key]
        public int MapCategoryId { get; set; }
        public string MapCategoryTitle { get; set; }
        public int? ParentMapCategoryId { get; set; }
        public string? Description { get; set; }
    }
}
