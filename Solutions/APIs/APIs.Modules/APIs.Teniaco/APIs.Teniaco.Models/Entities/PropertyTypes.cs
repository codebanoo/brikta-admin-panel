using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class PropertyTypes : BaseEntity
    {
        public PropertyTypes()
        {
            //FeaturesValues = new HashSet<FeaturesValues>();
            Properties = new HashSet<Properties>();
        }

        [Key]
        public int PropertyTypeId { get; set; }

        [StringLength(50)]
        public string PropertyTypeTilte { get; set; }

        //public virtual ICollection<FeaturesValues> FeaturesValues { get; set; }

        public virtual ICollection<Properties> Properties { get; set; }
    }
}
