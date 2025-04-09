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
    public partial class FormElementValues : BaseEntity
    {
        public FormElementValues()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormElementValueId { get; set; }

        [ForeignKey("FormElements")]
        public int FormElementId { get; set; }

        public string FormElementValue { get; set; }

        public FormElements FormElements { get; set; }
    }
}
