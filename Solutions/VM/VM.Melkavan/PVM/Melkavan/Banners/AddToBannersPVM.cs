using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Melkavan;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class AddToBannersPVM : BPVM
    {
        public BannersVM BannersVM{ get; set; }
        public IFormFile File { get; set; }
    }
}
