using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.PVM.Base;
using VM.Teniaco;

namespace VM.PVM.Teniaco
{
    public class AddToMapLayersJsonDataPVM : BPVM
    {
        public MapLayersVM MapLayersVM { get; set; }
        public List<string>? StrPolygonList { get; set; }

    }
}
