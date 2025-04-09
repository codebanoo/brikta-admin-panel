using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using VM.Public;
using VM.PVM.Base;


namespace VM.PVM.Public
{
    public class AddToDistrictFilesPVM : BPVM
    {
        public List<DistrictFilesVM> DistrictFilesVM { get; set; }

    }
}
