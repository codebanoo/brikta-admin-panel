using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class PropertyProjectTypes : BaseEntity
    {
        public PropertyProjectTypes()
        {
            PropertyProjects = new HashSet<PropertyProjects>();
        }

        [Key]
        public int PropertyProjectTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string PropertyProjectTypeTitle { get; set; }

        public virtual ICollection<PropertyProjects> PropertyProjects { get; set; }
    }
}
