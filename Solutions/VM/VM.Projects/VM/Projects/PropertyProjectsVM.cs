using System;
using System.Collections.Generic;
using System.Text;
using FrameWork;

namespace VM.Projects
{
    public class PropertyProjectsVM : BaseEntity
    {
        public PropertyProjectsVM()
        {

        }

        public int PropertyProjectId { get; set; }

        public long PropertyId { get; set; }

        public int? ParentPropertyProjectId { get; set; }

        public int PropertyProjectTypeId { get; set; }

        public bool IsPrivate { get; set; }

        public virtual List<OperationOnPropertyProjectsVM> OperationOnPropertyProjectsVM { get; set; }

        public virtual PropertyProjectTypesVM PropertyProjectTypesVM { get; set; }
    }
}
