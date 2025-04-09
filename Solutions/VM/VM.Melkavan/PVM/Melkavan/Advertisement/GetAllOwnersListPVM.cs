using System.Collections.Generic;
using VM.Console;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAllOwnersListPVM : BPVM
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Family { get; set; }
        public string? UserName { get; set; }
        public long OwnerId { get; set; }
        public List<CustomUsersVM>? CustomUsersVMList { get; set; }
    }
}
