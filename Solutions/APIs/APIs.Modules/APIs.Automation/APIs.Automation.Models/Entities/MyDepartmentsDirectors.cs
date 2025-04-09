using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIs.Automation.Models.Entities
{
    public partial class MyDepartmentsDirectors : BaseEntity
    {
        public MyDepartmentsDirectors()
        {
        }

        [Key]
        public int MyDepartmentDirectorId { get; set; }
        public int MyDepartmentId { get; set; }
        public long UserId { get; set; }
        public double? Credit { get; set; }
        public bool HasLimited { get; set; }//just child user seen defined educational group and so on
    }
}
