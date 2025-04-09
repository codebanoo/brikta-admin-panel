using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Automation
{
         public class UpdateCompanyPicturesPVM : BPVM
    {
        public int MyCompanyId { get; set; }
        public string CompanyLogo { get; set; }
        public string WaterMarkImage { get; set; }

    }
}
