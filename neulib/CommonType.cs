﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neulib
{
    public enum DaType
    {
        Int8 = 16, // UA_TYPES_SBYTE/VT_I1
        Int16 = 2, // UA_TYPES_INT16/VT_I2
        Int32B = 22, // UA_TYPES_INT32/VT_INT
        Int32 = 3, // UA_TYPES_INT32/VT_I4
        Int64 = 20, // UA_TYPES_INT64/VT_I8
        Float = 4, // UA_TYPES_FLOAT/VT_R4
        Double = 5, // UA_TYPES_DOUBLE/VT_R8
        UInt8 = 17, // UA_TYPES_BYTE/VT_UI1
        UInt16 = 18, // UA_TYPES_UINT16/VT_UI2
        UInt32B = 23, // UA_TYPES_UINT32/VT_UINT
        UInt32 = 19, // UA_TYPES_UINT32/VT_UI4
        UInt64 = 21, // UA_TYPES_UINT64/VT_UI8
        Date = 7, // UA_TYPES_DATETIME/VT_DATE
        String = 8, // UA_TYPES_STRING/VT_BSTR
        Bool = 11, // UA_TYPES_BOOLEAN/VT_BOOL
        Money = 6, // Money
        VtEmpty = 0, // UA_TYPES_STRING/VT_STRING
    }

    public enum Quality
    {
        GoodLocalOverrideValueForced = 216,
        Good = 192,
        UncertainValueFromMultipleSources = 88,
        UncertainEngineeringUnitsExceeded = 84,
        UncertainSensorNotAccurate = 80,
        UncertainLastUsableValue = 68,
        Uncertain = 64,
        BadWaitingForInitialData = 32,
        BadOutOfService = 28, // Also happens when item or group is inactive
        BadCommFailure = 24,
        BadLastKnowValuePassed = 20,
        BadSensorFailure = 16,
        BadDeviceFailure = 12,
        BadNotConnected = 8,
        BadConfigurationErrorInServer = 4,
        Bad = 0,
    }

    public class Item
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public dynamic Value { get; set; }
        public Quality Quality { get; set; }
        public DateTime Timestamp { get; set; }
    }


    public class Msg
    {
        public List<Item> Items { get; set; }
    }


    public class Tag
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }

        public Tag(string name, string id, string path)
        {
            Name = name;
            Id = id;
            Path = path;
        }
    }

    public class TagCollection : List<Tag>
    {
        public TagCollection() : base() { }
        public TagCollection(IEnumerable<Tag> collection) : base(collection) { }
        public TagCollection(int capacity) : base(capacity) { }
    }

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

    public delegate bool ValueWrite(Item item);
}
