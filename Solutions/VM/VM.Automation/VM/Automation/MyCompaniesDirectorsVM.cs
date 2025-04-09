using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Automation
{
    public class MyCompaniesDirectorsVM : BaseEntity
    {
        public int MyCompanyDirectorId { get; set; }
        public int MyCompanyId { get; set; }
        public long UserId { get; set; }
        public double? Credit { get; set; }
        public bool HasLimited { get; set; }//just child user seen defined educational group and so on
    }
}
