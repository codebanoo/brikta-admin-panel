using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VM.Automation
{
    public class MyDepartmentsVM : BaseEntity
    {
        private string lang = "fa";

        public MyDepartmentsVM()
        {

        }

        public int MyDepartmentId { get; set; }

        public int ParentId { get; set; }
        public int ParentType { get; set; }

        public int? OrgChartNodeId { get; set; }

        [Required]
        public string MyDepartmentName { get; set; }

        public string MyDepartmentRealName { get; set; }

        public DateTime? EstablishedEnDate { get; set; }//تاریخ تاسیس

        [NotMapped]
        public string EstablishedDateTime
        {
            get
            {
                if (this.EstablishedEnDate.HasValue)
                    return DateManager.ConvertFromDate(lang, this.EstablishedEnDate.Value);
                else
                    return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(lang) && !string.IsNullOrEmpty(value))
                    this.EstablishedEnDate = DateManager.ConvertToDate(lang, value);
            }
        }

        //public long? MyDepartmentDirectorId { get; set; }//آی دی مدیر بخش
        public long? UserId { get; set; }

        [NotMapped]
        public string DirectorName { get; set; }

        public string DutiesDescriptions { get; set; }

        //[NotMapped]
        //public MyCompaniesVM MyCompaniesVM { get; set; }
    }
}
