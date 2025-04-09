using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class TypeOfUses : BaseEntity
    {
        public TypeOfUses()
        {
            Properties = new HashSet<Properties>();
        }

        [Key]
        public int TypeOfUseId { get; set; }

        [Required]
        [StringLength(50)]
        public string TypeOfUseTitle { get; set; }

        public virtual ICollection<Properties> Properties { get; set; }
    }
}
