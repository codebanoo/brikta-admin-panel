using System;
using System.Collections.Generic;
using System.Text;
using FrameWork;

namespace VM.Teniaco
{
    public class TypeOfUsesVM : BaseEntity
    {
        public TypeOfUsesVM()
        {
            PropertiesVM = new HashSet<PropertiesVM>();
        }

        public int TypeOfUseId { get; set; }

        public string TypeOfUseTitle { get; set; }

        public virtual ICollection<PropertiesVM> PropertiesVM { get; set; }
    }
}
