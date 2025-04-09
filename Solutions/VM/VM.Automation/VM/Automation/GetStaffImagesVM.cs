using FrameWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace VM.Automation
{
    public class GetStaffImagesVM : BaseEntity
    {
        public string ContractImage { get; set; }
        public string CertificateImage { get; set; }
        public string NationalCodeImage { get; set; }
    }
}
