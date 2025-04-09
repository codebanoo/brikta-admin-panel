using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class GroupedConstructionProjectDailyPicturesVM
    {
        public YearKey YearKey { get; set; }
    }

    public class YearKey    { 
        public string YearFa { get; set; }

        public List<MonthKey> MonthKey{ get; set; }
    }

    public class MonthKey
    {
        public string MonthFa { get; set; }

        public List<ConstructionProjectDailyPicturesVM> ConstructionProjectDailyPicturesVMList { get; set; }
    }
}
