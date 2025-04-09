using System.ComponentModel.DataAnnotations.Schema;

namespace VM.Projects
{
    public class SumOfPrivateCostsVM
    {
        public long RowNumber { get; set; }

        public string CellData { get; set; }

        public double SumOfPrivateCost { get; set; }
        
    }
}
