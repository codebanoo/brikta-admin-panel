using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    [Keyless]
    public class InvestorWithFullName
    {
        public string Name { get; set; } = "";
        public string Family { get; set; } = "";
        public int InvestorId { get; set; }

        [NotMapped]
        public string FullName => $"{Name} {Family}";
        public override string ToString() => FullName;
    }
}
