using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Teniaco.Models.Entities
{
    public partial class PropertySelectedCallers : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertySelectedCallersId { get; set; }
        public long PropertyId { get; set; }
        public string? CallerType { get; set; }
        public string? AdvertiserNumber { get; set; }
        public string? AgencyNumber { get; set; }
    }
}
