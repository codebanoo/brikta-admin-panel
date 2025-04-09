using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Public
{
    public class UnitsOfMeasurementVM: BaseEntity
    {
        public int UnitOfMeasurementId { get; set; }

        public string UnitOfMeasurementTitle { get; set; }
    }
}
