using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class DocumentRootTypesVM : BaseEntity
    {
        public DocumentRootTypesVM()
        {
            PropertiesVM = new HashSet<PropertiesVM>();
        }

        public int DocumentRootTypeId { get; set; }

        public string DocumentRootTypeTitle { get; set; }

        public virtual ICollection<PropertiesVM> PropertiesVM { get; set; }
    }
}
