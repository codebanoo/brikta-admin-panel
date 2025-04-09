using FrameWork;
using System.ComponentModel.DataAnnotations;


namespace VM.Teniaco
{
    public class PositionsVM : BaseEntity
    {
        [Required(ErrorMessage = "سمت را انتخاب کنید")]
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
    }
}
