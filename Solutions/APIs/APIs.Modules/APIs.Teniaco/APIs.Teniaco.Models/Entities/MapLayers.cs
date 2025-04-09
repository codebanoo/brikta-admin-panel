using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public class MapLayers : BaseEntity
    {
        [Key]
        public int MapLayerId { get; set; }

        [ForeignKey("MapLayerCategories")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MapLayerCategoryId { get; set; }

        public string? StrPolygon { get; set; }

        public virtual MapLayerCategories MapLayerCategories { get; set; }

 
        public int? DistrictId { get; set; }
    }
}
