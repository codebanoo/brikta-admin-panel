using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Teniaco.Models.Entities
{
    public class PropertyBuyers : BaseEntity
    {
        [Key]
        public int PropertyBuyerId { get; set; }
        public long PersonId { get; set; }
        public long PropertyId { get; set; }
        [StringLength(500)]
        public string? BuyerDescription { get; set; }
    }
}
