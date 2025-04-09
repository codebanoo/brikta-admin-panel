using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public partial class TypeOfUseLayers : BaseEntity
    {
        public int TypeOfUseLayersId { get; set; }
        public string? TypeOfUseLayersName { get; set; }
    }
}
