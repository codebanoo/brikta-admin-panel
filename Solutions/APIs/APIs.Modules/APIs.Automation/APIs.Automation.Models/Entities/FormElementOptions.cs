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
    public partial class FormElementOptions : BaseEntity
    {
        public FormElementOptions()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormElementOptionId { get; set; }

        [ForeignKey("FormElements")]
        public int FormElementId { get; set; }

        public int FormElementOptionValue { get; set; }

        public string FormElementOptionText { get; set; }

        public FormElements FormElements { get; set; }
    }
}
