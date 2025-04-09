using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class PaymentsAndCostsVM
    {
        public int RowNumber { get; set; }

        public int Date { get; set; }

        public double? Payment { get; set; }

        public double? Cost { get; set; }
        public long? ConstructionProjectRepresentativeId { get; set; }
    }
}
