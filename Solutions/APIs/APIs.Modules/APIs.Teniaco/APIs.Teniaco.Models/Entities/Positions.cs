using FrameWork;
using System.ComponentModel.DataAnnotations;


namespace APIs.Teniaco.Models.Entities
{
    public partial class Positions : BaseEntity
    {
        [Key]
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
    }
}
