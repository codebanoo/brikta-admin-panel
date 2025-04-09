using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ProjectProgressDataVM
    {
        public long ConstructionProjectId { get; set; }

        public string Program { get; set; }//برنامه

        public string Operation { get; set; }//عملکرد

        public string Deviation { get; set; }//انحراف

        public string ProgramStart { get; set; }//شروع برنامه

        public string ProgramEnd { get; set; }//پایان برنامه
        public string ConstructionProjectTitle { get; set; }//نام پروژه

    }
}
