using VM.Console;
using System;
using System.Collections.Generic;
using System.Text;
using FrameWork;

namespace VM.Automation
{
    public class MyDepartmentsDirectorsVM : BaseEntity
    {
        public int MyDepartmentDirectorId { get; set; }
        public int MyDepartmentId { get; set; }
        public long UserId { get; set; }

        //public int EducationalGroupId { get; set; }

        public double? Credit { get; set; }

        public bool HasLimited { get; set; }//just child user seen defined educational group and so on

        //public bool IsFree { get; set; }
    }
}
