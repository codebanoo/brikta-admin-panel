using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Automation.Models.Entities
{
    public partial class Forms : BaseEntity
    {
        public Forms()
        {
            FormElements = new HashSet<FormElements>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormId { get; set; }

        public string FormName { get; set; }

        public string FormUsageIds { get; set; }

        public ICollection<FormElements> FormElements { get; set; }
    }
}
