using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Teniaco.Models.Entities
{
    public partial class DocumentTypes : BaseEntity
    {
        public DocumentTypes()
        {
            Properties = new HashSet<Properties>();
        }

        [Key]
        public int DocumentTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentTypeTitle { get; set; }

        public virtual ICollection<Properties> Properties { get; set; }
    }
}
