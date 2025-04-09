using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIs.Automation.Models.Entities
{
    public partial class DepartmentsStaff : BaseEntity
    {
        [Key]
        public int DepartmentStaffId { get; set; }
        public int MyDepartmentId { get; set; }
        public long UserId { get; set; }
        //public Staff Staff { get; set; }
        //public MyDepartments MyDepartments { get; set; }
        //public Users User { get; set; }
        //public EducationalGroups EducationalGroups { get; set; }
    }
}
