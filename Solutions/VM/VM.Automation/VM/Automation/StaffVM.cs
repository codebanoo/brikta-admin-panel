using FrameWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace VM.Automation
{
    public class StaffVM : BaseEntity
    {
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
    }
}
