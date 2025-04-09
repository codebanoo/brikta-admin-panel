using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Teniaco.Models.Entities
{
    public partial class Funds : BaseEntity
    {
        [Key]
        public int FundId { get; set; }
        public string? FundName { get; set; }
    }
}
