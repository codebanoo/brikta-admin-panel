using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Public
{
    public class PricesListIndicesVM : BaseEntity
    {
        public int PricesListIndicesId { get; set; }

       
        public int IndicesId { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public decimal Change { get; set; }
        public string PDate //{ get; set; }
        {
            get
            {

                return DateManager.ConvertFromDate("fa", this.Date);

            }
            set
            {
                    this.Date = DateManager.ConvertToDate("en", value).Value;
            }
        }
        public string IndicesName { get; set; }
        public string CreatorName { get; set; } 
    }
}
