using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Automation
{
    public class FormsVM : BaseEntity
    {
        public FormsVM()
        {
            FormElementsVM = new HashSet<FormElementsVM>();
        }

        public int FormId { get; set; }

        public string? FormName { get; set; }

        public string FormUsageIds { get; set; }

        public ICollection<FormElementsVM> FormElementsVM { get; set; }
    }
}
