using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class GetListOfProgressPicturesPVM : BPVM
    {
        public long? ConstructionProjectId { get; set; }
        public string? ProgressPictureTitle { get; set; }
    }
}
