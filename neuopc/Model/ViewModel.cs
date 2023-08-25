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

        private string _type;
        public string Type
        {
            get { return _type; }
            set { _type = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(Type))); }
        }


        private string _path;
        public string Path
        {
            get { return _path; }
            set { _type = value; PropertyChanged(this, new PropertyChangedEventArgs(nameof(Type))); }
        }

        public string ItemId
        {
            get
            {
                return _path.Substring(_path.IndexOf("/") + 1, _path.Length - _path.IndexOf("/") - 1).Replace("/", ".");
            }

        }

        public ViewModel(string name, string type, string path)
        {
            _name = name;
            _type = type;
            _path = path;
        }
    }
}
