using Microsoft.AspNetCore.Http;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class AddAgreementPVM : BPVM
    {
        public long ConstructionProjectId { get; set; }
        public string? AgreementTitle { get; set; }
        public string? AgreementDescription { get; set; }
        public string ContractType { get; set; }
        public IFormFile File { get; set; }
    }
}
