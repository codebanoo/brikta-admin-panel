using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan
{
    public class MelkavanPropertiesUsersProfileVM : BaseEntity
    {
        public long UserId { get; set; }
        public string Name { get; set; }

        public string Family { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string Mobile { get; set; }
        public string? Email { get; set; }
    }
}
