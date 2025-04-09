using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class PropertyProjectTypesVM : BaseEntity
    {
        public int PropertyProjectTypeId { get; set; }

        public string PropertyProjectTypeTitle { get; set; }

        public virtual List<PropertyProjectsVM> PropertyProjectsVM { get; set; }
    }
}
