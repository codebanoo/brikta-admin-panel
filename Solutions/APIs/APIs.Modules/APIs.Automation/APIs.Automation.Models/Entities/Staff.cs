using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIs.Automation.Models.Entities
{
    public partial class Staff : BaseEntity
    {
        public Staff()
        {
        }

        public int StaffId { get; set; }
        public long UserId { get; set; }
        public string ContractImage { get; set; }
        public string PersonalCode { get; set; }
        public string CertificateImage { get; set; }
        public string NationalCodeImage { get; set; }
        public string JobTitle { get; set; }
        public string Skill { get; set; }
        public bool IsMarried { get; set; }
        public int Dependants { get; set; }
        //public ICollection<DepartmentsStaff> DepartmentsStaff { get; set; }
    }
}
