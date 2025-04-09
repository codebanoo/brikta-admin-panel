using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    [NotMapped]
    public class ConstructionProjectDataTypeCounts
    {
        [Key]
        public long ConstructionProjectDataTypeCountId { get; set; }

        public long ConstructionProjectId { get; set; }

        public string DataType { get; set; }

        public int Count { get; set; }
    }
}
