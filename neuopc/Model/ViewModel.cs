using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace neuopc.Model
{
    internal class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(Name))); }
        }

        private string _path;
        public string Path
        {
            get { return _path; }
            set { _path = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(Type))); }
        }

        private string _itemId;
        public string ItemId
        {
            get { return _itemId; }
            set { _itemId = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(ItemId))); }

        }

        public ViewModel(string name, string nodeId, string path)
        {
            _name = name;
            _itemId = nodeId;
            _path = path;
        }
    }
}
