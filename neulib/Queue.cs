using System;
using System.Collections.Generic;
using System.Text;

namespace neulib
{
    public class QueueMsg
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string opc_tag { get; set; }
        public bool visible { get; set; }
        public int equipment_id { get; set; }
        public string unit { get; set; }
    }
}
