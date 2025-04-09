using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;
using VM.Automation;

namespace VM.PVM.Automation
{
    public class GetMyDepartmentDirectorWithMyDepartmentDirectorIdPVM : BPVM
    {
        public int MyDepartmentId { get; set; }
    }
}
