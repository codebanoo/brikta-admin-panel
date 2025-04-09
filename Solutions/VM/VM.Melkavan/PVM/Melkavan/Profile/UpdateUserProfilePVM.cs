using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Public;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class UpdateUserProfilePVM : BPVM
    {
        public long UserId { get; set; }
        public string? Name{ get; set; }
        public string? Family { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public int PersonTypeId { get; set; }
    }
}
