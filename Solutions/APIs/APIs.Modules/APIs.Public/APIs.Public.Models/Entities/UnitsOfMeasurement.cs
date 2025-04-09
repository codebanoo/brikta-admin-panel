using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public partial class UnitsOfMeasurement : BaseEntity
    {
        public int UnitOfMeasurementId { get; set; }

        public string UnitOfMeasurementTitle { get; set; }
    }
}
