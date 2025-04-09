using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetAttachementsByAgreementIdAndTableTitlePVM : BPVM
    {
        public long AgreeemntId { get; set; }
        public string TableTitle { get; set; }
    }
}
