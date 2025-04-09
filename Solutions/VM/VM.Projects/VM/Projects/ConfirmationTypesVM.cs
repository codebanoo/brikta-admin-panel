using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ConfirmationTypesVM : BaseEntity
    {
        public int ConfirmationTypeId { get; set; }
        public string? ConfirmationTypeTitle { get; set; }
    }
}
