using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public class PersonTypes : BaseEntity
    {
        public int PersonTypeId { get; set; }
        public string PersonTypeTitle { get; set; }
    }
}
