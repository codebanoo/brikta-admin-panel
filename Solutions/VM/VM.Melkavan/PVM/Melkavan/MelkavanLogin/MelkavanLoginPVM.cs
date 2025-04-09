using VM.Melkavan;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class MelkavanLoginPVM : BPVM
    {
        public MelkavanLoginVM MelkavanLoginVM { get; set; }

        public string? TempVerifyCode{ get; set; }
    }
}
