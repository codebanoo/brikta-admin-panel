using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class MyPropertiesCompareTopicVM : BaseEntity
    {
        public long PropertyId { get; set; }
        public string Title { get; set; } = "";

        public int PropertyTypeId { get; set; }
    }

}
