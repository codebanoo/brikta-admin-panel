using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class ElementTypes : BaseEntity
    {
        public ElementTypes()
        {
            Features = new HashSet<Features>();
        }

        [Key]
        public int ElementTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ElementTypeTitle { get; set; }

        //[Required]
        //[StringLength(50)]
        public string? ElementTypeKey { get; set; }

        public virtual ICollection<Features> Features { get; set; }
    }
}
