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
    public partial class PropertyOwners : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PropertyOwnerId { get; set; }

        //مالک
        public long OwnerId { get; set; }

        [ForeignKey("Properties")]
        public long PropertyId { get; set; }

        //دنگ
        public double Share { get; set; }

        //درصد دنگ
        public double SharePercent { get; set; }

        public string OwnerType { get; set; }

        public virtual Properties Properties { get; set; }
    }
}
