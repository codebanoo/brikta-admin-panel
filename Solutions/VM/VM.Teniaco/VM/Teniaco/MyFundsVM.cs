using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class MyFundsVM
    {
        //public int MyFundId { get; set; }

        //عنوان ملک ، پروژه
        public string? MyFundTitle { get; set; }

        //آخرین ارزش
        public double? MyFundPrice { get; set; }

        //رشد نسبت به اولین قیمت
        public double? Growth { get; set; }
    }
}
