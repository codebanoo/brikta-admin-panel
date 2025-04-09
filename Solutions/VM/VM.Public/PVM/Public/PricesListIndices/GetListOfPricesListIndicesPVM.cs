using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Public
{
    public class GetListOfPricesListIndicesPVM : BPVM
    {
        public int? IndicesId { get; set; }
        public DateTime? Bdate { get; set; }
        public DateTime? EDate { get; set; }
    }
}
