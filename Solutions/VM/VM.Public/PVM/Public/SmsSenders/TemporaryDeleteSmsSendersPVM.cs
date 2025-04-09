using System;
using System.Collections.Generic;
using System.Text;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class TemporaryDeleteSmsSendersPVM : BPVM
    {
        public long SmsSenderId { get; set; }
    }
}
