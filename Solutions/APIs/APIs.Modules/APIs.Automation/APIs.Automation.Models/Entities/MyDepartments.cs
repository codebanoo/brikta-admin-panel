using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIs.Automation.Models.Entities
{
    public partial class MyDepartments : BaseEntity
    {
        [Key]
        public int MyDepartmentId { get; set; }
        public int ParentId { get; set; }
        public string ParentType { get; set; }
        public int? OrgChartNodeId { get; set; }
        //[CustomRequired]
        public string MyDepartmentName { get; set; }
        public string MyDepartmentRealName { get; set; }
        public DateTime? EstablishedEnDate { get; set; }//تاریخ تاسیس
        //public long? MyDepartmentDirectorId { get; set; }//آی دی مدیر بخش
        public string DutiesDescriptions { get; set; }//وظایف
        //public MyCompanies MyCompanies { get; set; }
        //public MyDepartmentsDirectors MyDepartmentsDirectors { get; set; }
        //public ICollection<DepartmentsStaff> DepartmentsStaff { get; set; }
    }
}
