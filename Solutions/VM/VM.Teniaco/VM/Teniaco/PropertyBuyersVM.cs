using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Teniaco;

namespace VM.Teniaco.VM.Teniaco
{
    public class PropertyBuyersVM : BaseEntity
    {
        public int PropertyBuyerId {  get; set; }
        public long PersonId {  get; set; }
        public long PropertyId { get; set; }
        public string? BuyerDescription { get; set; }
    }
}
