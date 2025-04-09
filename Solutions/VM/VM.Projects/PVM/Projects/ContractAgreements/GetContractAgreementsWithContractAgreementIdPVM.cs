using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetContractAgreementsWithContractAgreementIdPVM : BPVM
    {
        public long ContractAgreementId { get; set; }
    }
}
