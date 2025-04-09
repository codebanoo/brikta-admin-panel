using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class DocumentOwnershipTypesVM : BaseEntity
    {
        public DocumentOwnershipTypesVM()
        {
            PropertiesVM = new HashSet<PropertiesVM>();
        }

        public int DocumentOwnershipTypeId { get; set; }

        public string DocumentOwnershipTypeTitle { get; set; }

        public virtual ICollection<PropertiesVM> PropertiesVM { get; set; }
    }
}
