using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAdvertisementsWithOwnerIdPVM : BPVM
    {
        public long OwnerId { get; set; }
        public string? PropertyCodeName { get; set; }
    }
}
