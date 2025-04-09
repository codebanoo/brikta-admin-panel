using System.Collections.Generic;
using VM.Console;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAllCosultantsListPVM : BPVM
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Family { get; set; }
        public string? UserName { get; set; }
        public long ConsultantId { get; set; }
        public List<CustomUsersVM>? CustomUsersVMList { get; set; }
    }
}
