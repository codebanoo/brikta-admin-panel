using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementOwners : BaseEntity
    {
        [Key]
        public int AdvertisementOwnerId { get; set; }

        //مالک
        public long OwnerId { get; set; }
        [ForeignKey("Advertisement")]
        public long AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }
        //دنگ
        public double Share { get; set; }

        //درصد دنگ
        public double SharePercent { get; set; }

        //نوع مالک
        //users
        //persons
        public string OwnerType { get; set; }
    }
}
