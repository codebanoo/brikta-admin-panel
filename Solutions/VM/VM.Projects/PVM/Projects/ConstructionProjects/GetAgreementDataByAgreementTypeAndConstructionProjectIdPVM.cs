using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetAgreementDataByAgreementTypeAndConstructionProjectIdPVM : BPVM
    {
        public long ConstructionProjectId { get; set; }
        public string ContractType { get; set; }
        public bool HaveAttachments { get; set; }
        public bool HaveConversations { get; set; }
    }
}
