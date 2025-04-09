using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetConfirmationAgreementsWithConfirmationAgreementIdPVM : BPVM
    {
        public long ConfirmationAgreementId { get; set; }
    }
}
