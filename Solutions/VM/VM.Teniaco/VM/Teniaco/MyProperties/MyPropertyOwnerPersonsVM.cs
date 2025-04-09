using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Public;

namespace VM.Teniaco
{
    public class MyPropertyOwnersVM : BaseEntity
    {
        public int PropertyOwnerId { get; set; }

        //مالک
        public long OwnerId { get; set; }

        public long PropertyId { get; set; }

        //دنگ
        public double Share { get; set; }

        //درصد دنگ
        public double SharePercent { get; set; }

        public virtual PersonsVM? OwnerPersons { get; set; }
    }
}
