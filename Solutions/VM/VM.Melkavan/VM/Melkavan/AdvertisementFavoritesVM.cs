using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan
{
    public class AdvertisementFavoritesVM : BaseEntity
    {
        public int AdvertisementFavoriteId { get; set; }
        public long AdvertisementId { get; set; }
        [NotMapped]
        public string? Type {  get; set; }
    }
}
