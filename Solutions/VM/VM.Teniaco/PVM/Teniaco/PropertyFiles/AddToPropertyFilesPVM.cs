using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;
using VM.Teniaco;

namespace VM.PVM.Teniaco
{
    public class AddToPropertyFilesPVM : BPVM
    {
        public List<PropertyFilesVM> PropertyFilesVMList { get; set; }
        public List<int>? DeletedPhotosIDs {  get; set; }
        public bool? IsMainChanged { get; set; }
        public long? PropertyId {  get; set; }

        //public List<PropertyFilesPVM> PropertyFilesPVMList { get; set; }
    }
}
