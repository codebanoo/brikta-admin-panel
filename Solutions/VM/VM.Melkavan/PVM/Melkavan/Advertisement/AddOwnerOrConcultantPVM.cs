using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class AddOwnerOrConcultantPVM : BPVM
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Family { get; set; }
        public string? Type{ get; set; }
        public long? UserIdCreator { get; set; }
    }
}
