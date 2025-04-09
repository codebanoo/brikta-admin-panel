using FrameWork;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIs.Public.Models.Entities
{
    public partial class ElementTypes : BaseEntity
    {
        //public ElementTypesPublic()
        //{
        //    //Features = new HashSet<Features>();
        //}

        [Key]
        public int ElementTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string ElementTypeTitle { get; set; }

        public string? ElementTypeKey { get; set; }

        //public virtual ICollection<Features> Features { get; set; }
    }
}
