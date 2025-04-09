using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class SumOfPublicCostsRepresentativesVM
    {
        public long RowNumber { get; set; }

        public string CellData { get; set; }

        public double SumOfPublicCost { get; set; }
        public long? ConstructionProjectRepresentativeId { get; set; }
    }
}
