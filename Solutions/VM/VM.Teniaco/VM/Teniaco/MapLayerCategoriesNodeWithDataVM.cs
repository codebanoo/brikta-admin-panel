using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    public class MapLayerCategoriesNodeWithDataVM
    {
        public string title { get; set; } = "";
        public bool folder { get; set; }

        public string? id { get; set; }
        public string? parent { get; set; }

        public string? key { get; set; }
        

        public List<MapLayerCategoriesNodeWithDataVM>? children { get; set; } = new List<MapLayerCategoriesNodeWithDataVM>() { };

    }
}

