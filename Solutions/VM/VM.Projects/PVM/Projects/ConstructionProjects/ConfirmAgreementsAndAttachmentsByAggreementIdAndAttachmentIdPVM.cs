using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class ConfirmAgreementsAndAttachmentsByAggreementIdAndAttachmentIdPVM : BPVM
    {
        public string ContractType { get; set; }
        public long TargetId { get; set; }
    }
}
