using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Teniaco.Models.Entities
{
    public class PropertiesViewers : BaseEntity
    {
        [Key]
        public int PropertiesViewersId { get; set; }
        public long PropertyId { get; set; }
    }
}
