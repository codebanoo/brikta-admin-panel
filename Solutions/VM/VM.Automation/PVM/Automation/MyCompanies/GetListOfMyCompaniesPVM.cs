using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Automation
{
    public class GetListOfMyCompaniesPVM : BPVM
    {

        public string MyCompanyName { get; set; }
        public string Address { get; set; }
        public string Phones { get; set; }
        public string CommercialCode { get; set; }
        public string Faxes { get; set; }
        public string MyCompanyRealName { get; set; }
        public string RegisterNumber { get; set; }
        public string PostalCode { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public string NationalCode { get; set; }
    }
}
