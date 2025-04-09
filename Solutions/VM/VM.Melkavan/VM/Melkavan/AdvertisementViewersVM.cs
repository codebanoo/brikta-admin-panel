using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan
{
    public class AdvertisementViewersVM : BaseEntity
    {
        public int AdvertisementViewersId { get; set; }
        public long AdvertisementId { get; set; }
        [NotMapped]
        public string? Type { get; set; }
    }
}
