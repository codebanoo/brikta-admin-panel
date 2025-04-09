using System;
using System.Collections.Generic;
using System.Text;
using VM.Public;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class AddToSmsSendersPVM : BPVM
    {
        public SmsSendersVM SmsSendersVM { get; set; }
    }
}
