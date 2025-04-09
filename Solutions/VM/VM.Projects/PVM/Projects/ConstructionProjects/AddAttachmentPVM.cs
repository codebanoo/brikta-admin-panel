using Microsoft.AspNetCore.Http;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class AddAttachmentPVM : BPVM
    {
        public long ConstructionProjectId { get; set; }
        public long AgreementId { get; set; }
        public string? AttachmentTitle { get; set; }
        public string? AttachmentDescription { get; set; }
        public string ContractType { get; set; }
        public IFormFile File { get; set; }
    }
}
