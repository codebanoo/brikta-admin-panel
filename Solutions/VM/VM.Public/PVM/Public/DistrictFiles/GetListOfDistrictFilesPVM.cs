using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetListOfDistrictFilesPVM : BPVM
    {
        public int DistrictId { get; set; }

        public string DistrictFileTitle { get; set; }

        public string DistrictFileType { get; set; }
    }
}
