using FrameWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace VM.Automation
{
    public class DepartmentsStaffVM : BaseEntity
    {
        public int DepartmentStaffId { get; set; }

        public int MyDepartmentId { get; set; }

        public long UserId { get; set; }
    }
}
