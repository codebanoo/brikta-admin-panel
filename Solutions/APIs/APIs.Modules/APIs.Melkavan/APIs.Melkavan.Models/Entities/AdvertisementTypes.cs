using FrameWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementTypes : BaseEntity
    {
        public AdvertisementTypes()
        {
            //FeaturesValues = new HashSet<FeaturesValues>();
            //AdvertisementDetails = new HashSet<AdvertisementDetails>();
        }

        [Key]
        public int AdvertisementTypeId { get; set; }

        [StringLength(50)]
        public string AdvertisementTypeTilte { get; set; }

        //public virtual ICollection<FeaturesValues> FeaturesValues { get; set; }

        //public virtual ICollection<AdvertisementDetails> AdvertisementDetails { get; set; }
    }
}
