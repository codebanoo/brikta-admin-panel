using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class DocumentTypesVM : BaseEntity
    {
        public DocumentTypesVM()
        {
            PropertiesVM = new HashSet<PropertiesVM>();
        }

        public int DocumentTypeId { get; set; }

        public string DocumentTypeTitle { get; set; }

        public virtual ICollection<PropertiesVM> PropertiesVM { get; set; }
    }
}
