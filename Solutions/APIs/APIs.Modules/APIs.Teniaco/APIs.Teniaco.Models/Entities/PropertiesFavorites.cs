using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Teniaco.Models.Entities
{
    public class PropertiesFavorites : BaseEntity
    {
        [Key]
        public int PropertiesFavoriteId { get; set; }
        public long PropertyId { get; set; }
    }
}
