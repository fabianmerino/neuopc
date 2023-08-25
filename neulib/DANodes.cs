using System;
using System.Collections.Generic;

namespace neulib
{
    [Serializable]
    public class DANodesReqMsg : MsgBase
    {
        public List<string> Nodes;
    }

    [Serializable]
    public class DANodesResMsg : MsgBase
    {
        public bool Result;
    }
}
