using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork
{
    public class FieldsVM
    {
        public string COLUMN_NAME { get; set; }

        public string DATA_TYPE { get; set; }

        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }

        public string IS_NULLABLE { get; set; }//YES or NO
    }
}
