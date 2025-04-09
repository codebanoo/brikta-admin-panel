using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Melkavan.Models.Entities
{
    public class Tags : BaseEntity
    {
        [Key]
        public int TagId { get; set; }
        public string TagTitle { get; set; }
    }
}
