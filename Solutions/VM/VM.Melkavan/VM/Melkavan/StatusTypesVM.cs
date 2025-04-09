using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan.VM.Melkavan
{
    public class StatusTypesVM : BaseEntity
    {
        public int StatusId { get; set; }
        public string StatusTitle { get; set; }
    }
}
