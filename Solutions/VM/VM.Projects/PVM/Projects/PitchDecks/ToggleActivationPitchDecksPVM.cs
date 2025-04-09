using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Projects
{
    public class ToggleActivationPitchDecksPVM : BPVM
    {
        public long PitchDeckId { get; set; }
    }
}
