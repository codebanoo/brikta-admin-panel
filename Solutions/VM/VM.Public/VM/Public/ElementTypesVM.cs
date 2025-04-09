using System;
using System.Collections.Generic;
using System.Text;
using FrameWork;


namespace VM.Public
{
    public class ElementTypesVM : BaseEntity
    {
        public int ElementTypeId { get; set; }

        public string ElementTypeTitle { get; set; }

        public string? ElementTypeKey { get; set; }

    }
}
