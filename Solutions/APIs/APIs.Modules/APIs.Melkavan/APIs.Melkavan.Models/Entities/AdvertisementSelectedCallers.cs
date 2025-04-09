using FrameWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementSelectedCallers : BaseEntity
    {
        [Key]
        public int AdvertisementSelectedCallersId { get; set; }
        [ForeignKey("Advertisement")]
        public long AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }
        public string? CallerType { get; set; }
        public string? AdvertiserNumber { get; set; }
        public string? AgencyNumber { get; set; }

    }
}
