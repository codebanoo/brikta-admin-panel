using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Melkavan
{
    [NotMapped]
    public class AdvertisementDataTypeCounts
    {
        [Key]
        public long AdvertisementDataTypeCountId { get; set; }

        public long AdvertisementId { get; set; }

        public string DataType { get; set; }

        public int Count { get; set; }
    }
}
