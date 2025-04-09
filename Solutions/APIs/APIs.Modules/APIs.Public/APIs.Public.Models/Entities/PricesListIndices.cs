using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public class PricesListIndices : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PricesListIndicesId { get; set; }  

        [Required]
        public int IndicesId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Change { get; set; }

    }
}
