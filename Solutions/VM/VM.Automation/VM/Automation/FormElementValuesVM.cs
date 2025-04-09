using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Automation
{
    public class FormElementValuesVM : BaseEntity
    {
        public int FormElementValueId { get; set; }

        public int FormElementId { get; set; }

        public string? FormElementValue { get; set; }

        public FormElementsVM? FormElementsVM { get; set; }
    }
}
