using System;

namespace neuopc
{
    public class OPCChangeModel
    {
        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private object _value;

        public object Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        private TagQuality _quality;

        public TagQuality Quality
        {
            get { return _quality; }
            set { _quality = value; }
        }

        private DateTime _timeStamp;

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
    }
}
