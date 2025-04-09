using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    [NotMapped]
    public class PropertyDataTypeCounts
    {
        [Key]
        public long PropertyDataTypeCountId { get; set; }

        public long PropertyId { get; set; }

        public string DataType { get; set; }

        public int Count { get; set; }

        public string RecordType { get; set; }
    }
}
