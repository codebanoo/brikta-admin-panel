using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Automation
{
    public class FormElementOptionsVM : BaseEntity
    {
        public int FormElementOptionId { get; set; }

        public int FormElementId { get; set; }

        public int FormElementOptionValue { get; set; }

        public string? FormElementOptionText { get; set; }

        public FormElementsVM? FormElementsVM { get; set; }
    }
}
