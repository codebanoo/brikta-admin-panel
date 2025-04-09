using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetConversationDataByAgreementTypeAndRecordIdPVM : BPVM
    {
        public long RecordId { get; set; }
        public string ContractType { get; set; }
    }
}
