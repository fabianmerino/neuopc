using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neuopc
{
    class TagItem
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private int _serverHandler;

        public int ServerHandler
        {
            get { return _serverHandler; }
            set { _serverHandler = value; }
        }
        private int _clientHandler;

        public int ClientHandler
        {
            get { return _clientHandler; }
            set { _clientHandler = value; }
        }
    }
}
