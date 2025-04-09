using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;

namespace VM.PVM.Teniaco
{
    public class PropertyFileUploadPVM : BPVM
    {
        public int PropertyFileId { get; set; }

        public string PropertyFileTitle { get; set; }

        public int PropertyFileOrder { get; set; }

        public IFormFile File { get; set; }
    }
}
