using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class HierarchyProjectProgressItemsVM
    {
        public string name { get; set; }

        public List<Data> data { get; set; }

        public long? ConstructionProjectRepresentativeId { get; set; }

    }

    public class Data
    {
        public string name { get; set; }

        public string? id { get; set; }

        public string? parent { get; set; }

        public string? start { get; set; }

        public string? end { get; set; }

        public string? dependency { get; set; }

        public string? owner { get; set; }

        //public string? milestone { get; set; }

        public Completed completed { get; set; }

        public int y { get; set; }

        //public Data? data { get; set; }
    }

    public class Completed
    {
        public double? amount { get; set; }

        public string? fill { get; set; }
    }
}
