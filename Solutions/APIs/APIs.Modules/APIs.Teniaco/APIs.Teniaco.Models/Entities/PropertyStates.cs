using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class PropertyStates : BaseEntity
    {
        public PropertyStates()
        {
            //Properties = new HashSet<Properties>();
        }

        [Key]
        public int PropertyStateId { get; set; }

        [Required]
        [StringLength(50)]
        public string PropertyStateTitle { get; set; }

        //public virtual ICollection<Properties> Properties { get; set; }
    }
}
