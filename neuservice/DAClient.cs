﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using OPCAutomation;
using System.Net;
using System.Linq;
using System.Threading;
using Serilog;
using System.Threading.Channels;

namespace neuservice
{
    public class Node
    {
        public string Name { get; set; }
        public DaType Type { get; set; }
        public OPCItem Item { get; set; }
    }

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

    public enum DaRights
    {
        Read = 1,
        Write = 2,
        Rw = 3,
    }

    public enum DaQuality
    {
        GoodLocalOverrideValueForced = 216,
        Good = 192,
        UncertainValueFromMultipleSources = 88,
        UncertainEngineeringUnitsExceeded = 84,
        UncertainSensorNotAccurate = 80,
        UncertainLastUsableValue = 68,
        Uncertain = 64,
        BadOutOfService = 28, // Also happens when item or group is inactive
        BadCommFailure = 24,
        BadLastKnowValuePassed = 20,
        BadSensorFailure = 16,
        BadDeviceFailure = 12,
        BadNotConnected = 8,
        Bad = 0,
        BadConfigurationErrorInServer = 4,
    }

    public enum MsgType
    {
        List = 0,
        Data = 1,
    }

    public class Item
    {
        public string Name { get; set; }
        public DaType Type { get; set; }
        public DaRights Rights { get; set; }
        public int ClientHandle { get; set; }
        public dynamic Value { get; set; }
        public DaQuality Quality { get; set; }
        public DateTime Timestamp { get; set; }
        public int Error { get; set; }
    }

    public class DaMsg
    {
        public MsgType Type { get; set; }
        public string Host { get; set; }
        public string Server { get; set; }
        public string Status { get; set; }
        public List<Item> Items { get; set; }
    }

    public class DAClient
    {
        private const int MaxRead = 100;
        private OPCServer server;
        private OPCGroup group;
        private readonly List<Node> nodes;
        private string hostName;
        private string serverName;
        private List<string> items;

        private Thread thread;
        private readonly object locker;
        private readonly object nodesLocker;
        private bool running;

        private readonly List<Channel<DaMsg>> slowChannels;
        private readonly List<Channel<DaMsg>> fastChannels;

        public DAClient()
        {
            locker = new object();
            nodesLocker = new object();
            running = false;
            nodes = new List<Node>();
            items = new List<string>();
            slowChannels = new List<Channel<DaMsg>>();
            fastChannels = new List<Channel<DaMsg>>();
        }

        public List<string> Items
        {
            get { return items; }
            set { items = value; }
        }


        public static List<string> GetHosts()
        {
            var list = new List<string>();
            var ipHost = Dns.GetHostEntry("127.0.0.1");
            list.Add(ipHost.HostName);
            return list;
        }

        public static List<string> GetServers(string host)
        {
            var list = new List<string>();
            try
            {
                var opcServer = new OPCServer();
                object servers = opcServer.GetOPCServers(host);
                list.AddRange(((Array)servers).Cast<string>());
            }
            catch (Exception exception)
            {
                Log.Warning($"get opc servers failed, error:{exception.Message}");
            }

            return list;
        }

        public static List<string> GetItems(OPCServer opcServer)
        {
            var list = new List<string>();
            try
            {
                var browser = opcServer.CreateBrowser();
                browser.ShowBranches();
                browser.ShowLeafs(true);
                list.AddRange(browser.Cast<string>().ToArray());
            }
            catch (Exception exception)
            {
                Log.Warning($"get opc items failed, error:{exception.Message}");
            }

            return list;
        }

        public void AddSlowChannel(Channel<DaMsg> channel)
        {
            if (null == channel) { return; }
            slowChannels.Add(channel);
        }

        public void AddFastChannel(Channel<DaMsg> channel)
        {
            if (null == channel) { return; }
            fastChannels.Add(channel);
        }

        private bool SetServer()
        {
            if (Connected())
            {
                Disconnect();
            }

            try
            {
                server = new OPCServer();
                server.Connect(serverName, hostName);
            }
            catch (Exception exception)
            {
                server = null;
                Log.Error($"connect to {hostName}/{serverName} failed, error:{exception.Message}");
                return false;
            }

            Log.Information($"Server {hostName}/{serverName} connected");
            return true;
        }

        private bool SetGroup()
        {
            try
            {
                server.OPCGroups.DefaultGroupIsActive = true;
                group = server.OPCGroups.Add("all");
                group.IsActive = true;
            }
            catch (Exception error)
            {
                group = null;
                Log.Error($"add group failed, msg:{error.Message}");
                return false;
            }

            return true;
        }

        public bool SetNodes()
        {
            if (null == items || items.Count == 0)
            {
                Log.Warning($"SetNodes: list of nodes is empty");
                OPCBrowser browser;
                try
                {
                    browser = server.CreateBrowser();
                    browser.ShowBranches();
                    browser.ShowLeafs(true);
                }
                catch (Exception error)
                {
                    Log.Error($"create browser failed, msg:{error.Message}");
                    return false;
                }

                lock (nodesLocker)
                {
                    nodes.Clear();
                    int index = 0;

                    foreach (var item in browser)
                    {
                        var node = new Node()
                        {
                            Name = item.ToString(),
                        };

                        try
                        {
                            node.Item = group.OPCItems.AddItem(node.Name, index);
                            node.Type = (DaType)node.Item.CanonicalDataType;
                            nodes.Add(node);
                            Log.Information($"add item success, name:{node.Name}, index:{index}");

                            index++;
                        }
                        catch (Exception exception)
                        {
                            Log.Warning($"add item failed, name:{item}, error:{exception.Message}");
                        }
                    }
                }
            }
            else
            {
                lock (nodesLocker)
                {
                    nodes.Clear();
                    int index = 0;

                    foreach (var item in items)
                    {
                        var node = new Node()
                        {
                            Name = item,
                        };

                        try
                        {
                            node.Item = group.OPCItems.AddItem(node.Name, index);
                            node.Type = (DaType)node.Item.CanonicalDataType;
                            nodes.Add(node);
                            Log.Information($"add item success, name:{node.Name}, index:{index}");

                            index++;
                        }
                        catch (Exception exception)
                        {
                            Log.Warning($"add item failed, name:{item}, error:{exception.Message}");
                        }
                    }
                }
            }



            return true;
        }


        public bool UpdateNodes()
        {
            if (null == items || items.Count == 0)
            {
                Log.Warning($"UodateNodes: list of nodes is empty");
                OPCBrowser browser;
                try
                {
                    browser = server.CreateBrowser();
                    browser.ShowBranches();
                    browser.ShowLeafs(true);
                }
                catch (Exception error)
                {
                    Log.Error($"create browser failed, msg:{error.Message}");
                    return false;
                }
                lock (nodesLocker)
                {
                    foreach (var item in browser)
                    {
                        var name = item.ToString();
                        if (nodes.Where(node => node.Name.Equals(name)).Count() > 0)
                        {
                            continue;
                        }

                        var node = new Node()
                        {
                            Name = name.ToString(),
                        };

                        try
                        {
                            var last = nodes.LastOrDefault();
                            var lastHandle = last.Item.ClientHandle;
                            var index = null == last ? 0 : (lastHandle + 1);
                            node.Item = group.OPCItems.AddItem(node.Name, index);
                            node.Type = (DaType)node.Item.CanonicalDataType;
                            nodes.Add(node);
                            Log.Information($"update item success, name:{item}, index:{index}");
                        }
                        catch (Exception exception)
                        {
                            Log.Warning($"update item failed, name:{item}, error:{exception.Message}");
                        }
                    }
                }
            }
            else
            {
                lock (nodesLocker)
                {
                    foreach (var item in items)
                    {
                        var name = item.ToString();
                        if (nodes.Where(node => node.Name.Equals(name)).Count() > 0)
                        {
                            continue;
                        }

                        var node = new Node()
                        {
                            Name = name.ToString(),
                        };

                        try
                        {
                            var last = nodes.LastOrDefault();
                            var lastHandle = last.Item.ClientHandle;
                            var index = null == last ? 0 : (lastHandle + 1);
                            node.Item = group.OPCItems.AddItem(node.Name, index);
                            node.Type = (DaType)node.Item.CanonicalDataType;
                            nodes.Add(node);
                            Log.Information($"update item success, name:{item}, index:{index}");
                        }
                        catch (Exception exception)
                        {
                            Log.Warning($"update item failed, name:{item}, error:{exception.Message}");
                        }
                    }
                }
            }

            return true;
        }

        private bool SetItems()
        {
            List<Item> items;

            lock (nodesLocker)
            {
                items = (from node in nodes
                         let name = node.Name
                         select new Item
                         {
                             Name = name,
                             Type = (DaType)node.Item.CanonicalDataType,
                             Rights = (DaRights)node.Item.AccessRights,
                             ClientHandle = node.Item.ClientHandle,
                             Quality = 0,
                             Timestamp = DateTime.Now,
                         }).ToList();

            }

            // write to the slow channels
            foreach (var channel in slowChannels)
            {
                var msg = new DaMsg
                {
                    Type = MsgType.List,
                    Items = items,
                };
                channel.Writer.TryWrite(msg);
            }

            // write to the fast channels
            foreach (var channel in fastChannels)
            {
                var msg = new DaMsg
                {
                    Type = MsgType.List,
                    Items = items,
                };
                channel.Writer.TryWrite(msg);
            }

            return true;
        }

        private bool SetChange()
        {
            if (null == group) { return false; }
            server.OPCGroups.DefaultGroupDeadband = 0;
            server.OPCGroups.DefaultGroupUpdateRate = 200;
            group.IsSubscribed = true;
            group.UpdateRate = 200;
            group.DataChange += GroupDataChange;
            return true;
        }

        private void SetNull()
        {
            server = null;
            group = null;

            lock (nodesLocker)
            {
                nodes.Clear();
            }
        }

        private bool Connect()
        {
            if (false == SetServer()) { return false; }

            if (false == SetGroup()) { return false; }

            if (false == SetNodes()) { return false; }

            if (false == SetItems()) { return false; }

            if (false == SetChange()) { return false; }

            return true;
        }

        private bool Connected()
        {
            if (null == server) { return false; }

            int state;
            try
            {
                state = server.ServerState;
            }
            catch (Exception exception)
            {
                state = -1;
                Log.Error($"server state get error: {exception.Message}");
            }

            return 0 == state;
        }

        private bool Read()
        {

            bool isError = false;
            lock (nodesLocker)
            {
                int count = null == nodes ? 0 : nodes.Count;
                int t1 = count / MaxRead;
                int t2 = (count % MaxRead) == 0 ? 0 : 1;
                int times = t1 + t2;
                for (int i = 0; i < times; i++)
                {
                    var tempNodes = nodes.Skip(i * MaxRead).Take(MaxRead).ToList();
                    var list = tempNodes.Select(node => node.Item.ServerHandle).ToList();
                    list.Insert(0, 0);
                    Array handles = list.ToArray();

                    Array values = null;
                    Array errors = null;
                    dynamic qualities = null;
                    dynamic timestamps = null;
                    var msg = new DaMsg
                    {
                        Type = MsgType.Data,
                        Host = hostName,
                        Server = serverName,
                        Items = new List<Item>(),
                    };
                    try
                    {
                        group.SyncRead(1, tempNodes.Count, ref handles, out values, out errors, out qualities, out timestamps);
                        for (int j = 0; j < tempNodes.Count; j++)
                        {
                            var n = tempNodes[j];
                            var item = new Item
                            {
                                Name = n.Name,
                                ClientHandle = n.Item.ClientHandle,
                                Type = n.Type,
                                Value = values.GetValue(j + 1),
                                Rights = (DaRights)n.Item.AccessRights,
                                Quality = (DaQuality)Convert.ToInt32(((Array)qualities).GetValue(j + 1)),
                                Error = Convert.ToInt32(errors.GetValue(j + 1)),
                                Timestamp = Convert.ToDateTime(((Array)timestamps).GetValue(j + 1)).ToLocalTime(),
                            };
                            msg.Items.Add(item);
                        }

                        msg.Status = "connected";
                    }
                    catch (Exception exception)
                    {
                        isError = true;
                        msg.Items.Clear();
                        for (int j = 0; j < tempNodes.Count; j++)
                        {
                            var n = tempNodes[j];
                            var item = new Item
                            {
                                Name = n.Name,
                                ClientHandle = n.Item.ClientHandle,
                                Type = n.Type,
                                Rights = (DaRights)n.Item.AccessRights,
                                Quality = DaQuality.Bad,
                                Error = 1001,
                            };
                            msg.Items.Add(item);
                        }

                        msg.Status = "disconnected";
                        Log.Error($"sync read group error: {exception.Message}");
                    }

                    // write to the slow channels
                    foreach (var channel in slowChannels)
                    {
                        channel.Writer.TryWrite(msg);
                    }

                    // write to the fast channels
                    foreach (var channel in fastChannels)
                    {
                        channel.Writer.TryWrite(msg);
                    }
                }
            }

            return !isError;
        }

        private void Disconnect()
        {
            if (null == server)
            {
                return;
            }

            try
            {
                group.DataChange -= GroupDataChange;
                server.Disconnect();
                Log.Information($"disconnect succeed");
            }
            catch (Exception exception)
            {
                Log.Error($"disconnect error:{exception.Message}");
            }
        }

        private void GroupDataChange(int transactionId, int numItems, ref Array clientHandles, ref Array itemValues, ref Array qualities, ref Array timeStamps)
        {
            var items = new List<Item>();
            for (var i = 1; i <= numItems; i++)
            {
                var handle = Convert.ToInt32(clientHandles.GetValue(i));

                Node node;
                lock (nodesLocker)
                {
                    node = nodes.FirstOrDefault(n => n.Item.ClientHandle == handle);
                }

                if (null == node)
                {
                    continue;
                }

                var item = new Item
                {
                    Name = node.Name,
                    ClientHandle = handle,
                    Type = node.Type,
                    Value = itemValues.GetValue(i),
                    Rights = (DaRights)node.Item.AccessRights,
                    Quality = (DaQuality)Convert.ToInt32(qualities.GetValue(i)),
                    Timestamp = Convert.ToDateTime(timeStamps.GetValue(i)).ToLocalTime(),
                };

                items.Add(item);
            }

            // write Fast channels
            foreach (var channel in fastChannels)
            {
                var msg = new DaMsg
                {
                    Type = MsgType.Data,
                    Items = items,
                };
                channel.Writer.TryWrite(msg);
            }
        }

        public bool Write(Item item)
        {
            Node writeNode;
            lock (nodesLocker)
            {
                writeNode = nodes?.FirstOrDefault(node => node.Name == item.Name);
            }

            if (null != writeNode)
            {
                try
                {
                    writeNode.Item.Write(item.Value);
                }
                catch (Exception exception)
                {
                    Log.Error($"write node failed, name:{writeNode.Name}, value:{item.Value}, error:{exception.Message}");
                    return false;
                }

                Log.Information($"write node succeed, name:{writeNode.Name}, value:{item.Value}");
                return true;
            }
            else
            {
                Log.Error($"write node failed, name:{item.Name}, value:{item.Value}");
                return false;
            }
        }

        private void Refresh()
        {
            // First connect
            if (false == Connect())
            {
                return;
            }

            while (true)
            {
                lock (locker)
                {
                    if (!running)
                    {
                        break;
                    }
                }

                if (!Read())
                {
                    Connect();
                }

                UpdateNodes();
                Thread.Sleep(1000);
            }

            Disconnect();
        }

        public void Open(string host, string name, List<string> listOfNodes = null)
        {
            hostName = host;
            serverName = name;
            items = listOfNodes;

            lock (locker)
            {
                if (running)
                {
                    if (null != thread)
                    {
                        running = false;
                    }
                }
            }

            lock (locker) { running = true; }
            var ts = new ThreadStart(Refresh);
            thread = new Thread(ts);
            thread.Start();
        }

        public void Close()
        {
            lock (locker) { running = false; }
            thread?.Join();
        }

        public bool TestServer(string host, string server)
        {
            var test_server = new OPCServer();
            try
            {
                test_server.Connect(server, host);
            }
            catch (Exception exception)
            {
                Log.Error($"connect to {hostName}/{serverName} failed, error:{exception.Message}");
                return false;
            }

            try
            {
                test_server.Disconnect();
            }
            catch (Exception exception)
            {
                Log.Warning($"disconnect to {hostName}/{serverName} failed, error:{exception.Message}");
            }

            return true;
        }
    }
}
