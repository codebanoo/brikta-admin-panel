using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Automation.Models.Entities
{
    public class MyCompaniesDirectors : BaseEntity
    {
        public MyCompaniesDirectors()
        {
        }

        [Key]
        public int MyCompanyDirectorId { get; set; }
        public int MyCompanyId { get; set; }
        public long UserId { get; set; }
        public double? Credit { get; set; }
        public bool HasLimited { get; set; }//just child user seen defined educational group and so on
    }
}
