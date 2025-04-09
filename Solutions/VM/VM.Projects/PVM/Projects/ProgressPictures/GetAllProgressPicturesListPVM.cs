using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetAllProgressPicturesListPVM : BPVM
    {
        public long? ConstructionProjectId { get; set; }
        public string? ProgressPictureTitle { get; set; }
    }
}
