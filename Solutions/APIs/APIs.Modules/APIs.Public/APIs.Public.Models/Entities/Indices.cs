using FrameWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public partial class Indices : BaseEntity
    {
        [Key]
        public int IndiceId { get; set; }

        [Required]

        public string Name { get; set; }


    }
}
