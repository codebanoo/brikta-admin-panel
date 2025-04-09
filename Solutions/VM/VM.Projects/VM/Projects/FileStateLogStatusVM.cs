using FrameWork;

namespace VM.Projects
{
    public class FileStateLogStatusVM : BaseEntity
    {
        public long AgreementId { get; set; }
        public bool IsView { get; set; }
        public bool IsConfirm { get; set; }
    }
}
