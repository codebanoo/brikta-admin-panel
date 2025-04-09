using FrameWork;
using System.ComponentModel.DataAnnotations;



namespace APIs.Teniaco.Models.Entities
{
    public class MapLayerCategories : BaseEntity
    {
        [Key]
        public int MapLayerCategoryId { get; set; }
        public string MapLayerCategoryTitle { get; set; }
        public int? ParentMapLayerCategoryId { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
