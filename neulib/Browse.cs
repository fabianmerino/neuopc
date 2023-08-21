using System;
using System.Collections.Generic;
using System.Text;

namespace neulib
{
    [Serializable]
    public class BrowseReqMsg : MsgBase
    {
        public string Path;
    }

    [Serializable]
    public class BrowseResMsg : MsgBase
    {
        public string[] Paths;
    }

}
