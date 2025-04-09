using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;
using VM.Projects;

namespace VM.PVM.Projects
{
    public class AddToContractAgreementsPVM : BPVM
    {
        public ContractAgreementsVM ContractAgreementsVM { get; set; }
        public IFormFile? File { get; set; }
    }
}
