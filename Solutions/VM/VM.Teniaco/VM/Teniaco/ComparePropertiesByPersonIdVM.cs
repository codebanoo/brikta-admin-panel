using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class ComparePropertiesByPersonIdVM : BaseEntity
    {
        public long PropertyId { get; set; }
        public int PropertyTypeId { get; set; }
        public string PropertyCodeName { get; set; }
        public string FullName { get; set; }

        //public string Name{ get; set; }

        //public string Family { get; set; }

        // public string Title { get; set; } = "";


    }

}
