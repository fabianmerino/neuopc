using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Serilog;
using neulib;
using neuopc.Model;
using System.ComponentModel;

namespace neuopc
{
    public partial class MainForm : Form
    {
        private readonly Task task;
        private readonly SubProcess subProcess;
        private bool running;
        private readonly OPCClientWrapper opcClient;
        private readonly BindingList<ViewModel> dataItems = new BindingList<ViewModel>();
        private TagTreeNode treeNode;

        public MainForm()
        {
            InitializeComponent();
            opcClient = new OPCClientWrapper();
            subProcess = new SubProcess();
            running = true;

            treeViewDaBrowse.AfterCollapse += TreeViewDABrowse_AfterCollapse;
            treeViewDaBrowse.AfterExpand += TreeViewDABrowse_AfterExpand;
            treeViewDaBrowse.DoubleClick += TreeViewDABrowse_DoubleClick;

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = dataItems;

            OnConnectChanged(false);
            UpdateDataGridState();
        }
        private void DataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            UpdateDataGridState();
        }

        private void UpdateDataGridState()
        {
            var has = dataGridView1.Rows.Count > 0;
            btnRemoveItem.Enabled = has;
            btnRemoveAll.Enabled = has;
        }

        private void OnConnectChanged(bool connect)
        {
            btnAddNode.Enabled = connect;
            btnRemoveAll.Enabled = connect;
            btnRemoveItem.Enabled = connect;

            if (!connect)
            {
                treeViewDaBrowse.Nodes.Clear();
            }
        }

        private void TreeViewDABrowse_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0) return;
            e.Node.ImageIndex = 2;
            e.Node.SelectedImageIndex = 3;
        }

        private void TreeViewDABrowse_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count == 0) return;

            var tag = e.Node.Tag?.ToString();
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            var subNode = treeNode.FindNodeByPath(tag);
            if (null == subNode || subNode.IsLeaf)
            {
                e.Node.Nodes.Clear();
                return;
            }

            if (subNode.Nodes.Count > 0 && subNode.Nodes.Any(a => a.IsLeaf))
            {
                return;
            }

            // Remove all children
            e.Node.Nodes.Clear();

            // Get the children
            subNode = opcClient.GetLeaf(subNode);
            if (null == subNode || 0 == subNode.Nodes.Count)
            {
                subNode.IsLeaf = true;
                return;
            }
            Dictionary<string, TreeNode> tree = new Dictionary<string, TreeNode>();
            NodeTree(subNode, tree);
            var childs = tree.Where(x => subNode.Nodes.Any(a => a.FullPath == x.Key)).Select(x => x.Value).ToArray();


            // // Add the children
            e.Node.ImageIndex = 4;
            e.Node.SelectedImageIndex = 5;
            e.Node.Nodes.AddRange(childs);
        }


        private void TreeViewDABrowse_DoubleClick(object sender, EventArgs e)
        {
            if (treeViewDaBrowse.SelectedNode == null || treeViewDaBrowse.SelectedNode.Nodes.Count > 0) return;
            var selectedNode = treeViewDaBrowse.SelectedNode;
            selectedNode.Nodes.Add(new TreeNode("Loading..."));
            selectedNode.Expand();
            // Monitor(treeViewDaBrowse.SelectedNode);
        }

        private void Monitor(TreeNode node)
        {
            // if (node.Nodes.Count > 0)
            // {
            //     foreach (TreeNode item in node.Nodes) Monitor(item);
            // }
            // else
            // {
            var tag = node.Tag?.ToString();
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            // Remove all children
            node.Nodes.Clear();
            // e.Node.ImageIndex = 0;
            // e.Node.SelectedImageIndex = 1;

            var subNode = treeNode.FindNodeByPath(tag);
            if (null == subNode || subNode.IsLeaf)
            {
                return;
            }

            // Get the children
            subNode = opcClient.GetLeaf(subNode);
            if (null == subNode || 0 == subNode.Nodes.Count)
            {
                subNode.IsLeaf = true;
                return;
            }
            Dictionary<string, TreeNode> tree = new Dictionary<string, TreeNode>();
            NodeTree(subNode, tree);
            var childs = tree.Where(x => subNode.Nodes.Any(a => a.FullPath == x.Key)).Select(x => x.Value).ToArray();

            // Add the children
            node.Nodes.AddRange(childs);
            node.Expand();

            // Add to listview
            // ListViewItem lvi = new ListViewItem();
            // lvi.Text = node.Text;
            // lvi.SubItems.Add(node.Tag.ToString());
            // }
        }


        private void ResetListView(List<DataItem> list)
        {
            try
            {
                Action<List<DataItem>> action = (data) =>
                {
                    var items = MainListView.Items;
                    foreach (var item in data)
                    {
                        ListViewItem li = MainListView.Items.Cast<ListViewItem>().FirstOrDefault(x => x.Text == item.Name);
                        if (null == li)
                        {
                            MainListView.BeginUpdate();
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = item.Name;
                            lvi.SubItems.Add(item.Type); // type
                            lvi.SubItems.Add(item.Right); // rights
                            lvi.SubItems.Add(item.Value); // value
                            lvi.SubItems.Add(item.Quality); // quality
                            lvi.SubItems.Add(item.Error); // error
                            lvi.SubItems.Add(item.Timestamp); // timestamp
                            lvi.SubItems.Add(item.ClientHandle); // handle
                            MainListView.Items.Add(lvi);
                            MainListView.EndUpdate();
                        }
                        else
                        {
                            li.SubItems[3].Text = item.Value;
                            li.SubItems[4].Text = item.Quality;
                            li.SubItems[5].Text = item.Error;
                            li.SubItems[6].Text = item.Timestamp;
                        }
                    }
                };

                Invoke(action, list);
            }
            catch (Exception exception)
            {
                Log.Error($"reset list view error: {exception.Message}");
            }
        }

        private void TestGetDatas()
        {
            while (running)
            {
                Thread.Sleep(3000);

                var req = new DataReqMsg();
                req.Type = MsgType.DADataReq;
                var buff = Serializer.Serialize<DataReqMsg>(req);
                subProcess.Request(in buff, out byte[] result);

                if (null != result)
                {
                    try
                    {
                        var requestMsg = Serializer.Deserialize<DataResMsg>(result);
                        ResetListView(requestMsg.Items);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"get datas error:{ex.Message}");
                    }
                }
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            NotifyIcon.Visible = true;
            var config = ConfigUtil.LoadConfig("neuopc.json");
            DAHostComboBox.Text = config.DAHost;
            DAServerComboBox.Text = config.DAServer;

            UAPortTextBox.Text = config.UAUrl;
            UAUserTextBox.Text = config.UAUser;
            UAPasswordTextBox.Text = config.UAPassword;
            CheckBox.Checked = config.AutoConnect;

            if (string.IsNullOrEmpty(UAPortTextBox.Text))
            {
                UAPortTextBox.Text = "opc.tcp://localhost:48401";
            }

            if (string.IsNullOrEmpty(UAUserTextBox.Text))
            {
                UAUserTextBox.Text = "admin";
            }

            if (string.IsNullOrEmpty(UAPasswordTextBox.Text))
            {
                UAPasswordTextBox.Text = "123456";
            }
            if (CheckBox.Checked)
            {
                subProcess.SetDAArguments(DAHostComboBox.Text, DAServerComboBox.Text);
                subProcess.SetUAArguments(UAPortTextBox.Text, UAUserTextBox.Text, UAPasswordTextBox.Text);

                SwitchButton.Text = "Stop";

                DAHostComboBox.Enabled = false;
                DAServerComboBox.Enabled = false;
                TestButton.Enabled = false;

                UAPortTextBox.Enabled = false;
                UAUserTextBox.Enabled = false;
                UAPasswordTextBox.Enabled = false;
            }
            else
            {
                subProcess.SetDAArguments("", "");
                subProcess.SetUAArguments("", "", "");

                SwitchButton.Text = "Start";
            }

            subProcess.Daemon();
            var ts = new ThreadStart(TestGetDatas);
            var thread = new Thread(ts);
            thread.Start();
        }

        private void DAServerComboBox_DropDown(object sender, EventArgs e)
        {
            DAServerComboBox.Text = string.Empty;
            DAServerComboBox.Items.Clear();

            var req = new DAServerReqMsg
            {
                Type = MsgType.DAServersReq,
                Host = DAHostComboBox.Text
            };
            var buff = Serializer.Serialize<DAServerReqMsg>(req);
            subProcess.Request(in buff, out byte[] result);
            if (null != result)
            {
                var res = Serializer.Deserialize<DAServerResMsg>(result);
                var list = res.Servers;
                DAServerComboBox.Items.AddRange(list.ToArray());
                if (0 < DAServerComboBox.Items.Count)
                {
                    DAServerComboBox.SelectedIndex = 0;
                }
            }
        }

        private void DAHostComboBox_DropDown(object sender, EventArgs e)
        {
            DAHostComboBox.Text = string.Empty;
            DAHostComboBox.Items.Clear();

            var req = new DAHostsReqMsg
            {
                Type = MsgType.DAHostsReq
            };
            var buff = Serializer.Serialize<DAHostsReqMsg>(req);
            subProcess.Request(in buff, out byte[] result);
            if (null != result)
            {
                var res = Serializer.Deserialize<DAHostsResMsg>(result);
                var list = res.Hosts;
                DAHostComboBox.Items.AddRange(list.ToArray());
                if (0 < DAHostComboBox.Items.Count)
                {
                    DAHostComboBox.SelectedIndex = 0;
                }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Do you want to exit the program?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (DialogResult.Cancel == result)
            {
                e.Cancel = true;
                return;
            }

            //this.Hide();
            //e.Cancel = true;

            //client.Close();
            //channel.Writer.Complete();
            //task.Wait();
            //server.Stop();
            //NotifyIcon.Dispose();

            Log.Information("exit neuopc");

            running = false;
            subProcess.Stop();

            NotifyIcon.Dispose();
            Environment.Exit(0);
        }

        private void MainListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewItem row = listView.GetItemAt(e.X, e.Y);
            ListViewItem.ListViewSubItem col = row.GetSubItemAt(e.X, e.Y);
            string strText = col.Text;
            try
            {
                Clipboard.SetDataObject(strText);
            }
            catch (System.Exception ex)
            {
                Log.Error($"clipboard error:{ex.Message}");
            }
        }

        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Log.Information("exit neuopc");

            var result = MessageBox.Show("Do you want to exit the program?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (DialogResult.Cancel == result)
            {
                return;
            }


            running = false;
            subProcess.Stop();
            Environment.Exit(0);
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            /*
             var req = new ConnectTestReqMsg
            {
                Type = MsgType.DAConnectTestReq,
                Host = DAHostComboBox.Text,
                Server = DAServerComboBox.Text
            };
            var buff = Serializer.Serialize<ConnectTestReqMsg>(req);
            subProcess.Request(in buff, out byte[] result);
            if (null != result)
            {
                var res = Serializer.Deserialize<ConnectTestResMsg>(result);
                if (res.Result)
                {
                    UALabel.Text = "Connection tested successfully";
                    UALabel.ForeColor = Color.Green;
                }
                else
                {
                    UALabel.Text = "Connection tested failed";
                    UALabel.ForeColor = Color.Red;
                }
            }
            */
            // Conect OPC DA server and get items from OPCBrowser

            try
            {
                var host = DAHostComboBox.Text;
                var server = DAServerComboBox.Text;

                opcClient.Init(host, server);
                if (!opcClient.IsOPCServerConnected())
                {
                    Log.Error($"Connect to {host} {server} failed");
                    return;
                }

                OnConnectChanged(true);
                treeViewDaBrowse.Nodes.Clear();
                treeNode = opcClient.GetTree();

                if (null == treeNode)
                {
                    return;
                }

                var root = new TreeNode(treeNode.Name) { ImageIndex = 0, SelectedImageIndex = 1 };
                treeViewDaBrowse.Nodes.Add(root);

                Dictionary<string, TreeNode> tree = new Dictionary<string, TreeNode>();
                NodeTree(treeNode, tree);

                var nodes = tree.Where(x => treeNode.Nodes.Any(a => a.FullPath == x.Key)).Select(x => x.Value).ToArray();
                root.Nodes.AddRange(nodes);
                root.Expand();

            }
            catch (Exception exception)
            {
                Log.Error($"Read items error: {exception.Message}");
            }
        }

        private void NodeTree(TagTreeNode root, Dictionary<string, TreeNode> tree, string parent = null)
        {
            foreach (var item in root.Nodes)
            {
                TreeNode node;

                if (item.IsLeaf)
                {
                    node = new TreeNode(item.Name) { Name = item.Name, Tag = item.FullPath, ImageIndex = 0, SelectedImageIndex = 1, Checked = false };
                }
                else
                {
                    node = new TreeNode(item.Name) { Name = item.Name, Tag = item.FullPath, ImageIndex = 2, SelectedImageIndex = 3 };
                }

                tree.Add(item.FullPath, node);

                if (!item.IsLeaf && item.Nodes != null && item.Nodes.Any())
                {
                    NodeTree(item, tree, item.FullPath);
                }

                if (parent != null && node != null) tree[parent].Nodes.Add(node);
            }
        }


        private void SwitchButton_Click(object sender, EventArgs e)
        {
            if (SwitchButton.Text.Equals("Start"))
            {
                // DA start
                var req1 = new ConnectReqMsg
                {
                    Type = neulib.MsgType.DAConnectReq,
                    Host = DAHostComboBox.Text,
                    Server = DAServerComboBox.Text
                };
                var buff1 = Serializer.Serialize<ConnectReqMsg>(req1);
                subProcess.Request(in buff1, out byte[] result1);
                if (null != result1)
                {
                    var res1 = Serializer.Deserialize<ConnectResMsg>(result1);
                    Log.Information($"connect to da, code:{res1.Code}, msg:{res1.Msg}");
                }

                // UA start
                var req2 = new UAStartReqMsg
                {
                    Type = neulib.MsgType.UAStartReq,
                    Url = UAPortTextBox.Text,
                    User = UAUserTextBox.Text,
                    Password = UAPasswordTextBox.Text
                };
                var buff2 = Serializer.Serialize<UAStartReqMsg>(req2);
                subProcess.Request(in buff2, out byte[] result2);
                if (null != result2)
                {
                    var res2 = Serializer.Deserialize<UAStartResMsg>(result2);
                }

                SwitchButton.Text = "Stop";

                DAHostComboBox.Enabled = false;
                DAServerComboBox.Enabled = false;
                TestButton.Enabled = false;

                UAPortTextBox.Enabled = false;
                UAUserTextBox.Enabled = false;
                UAPasswordTextBox.Enabled = false;
            }
            else
            {
                // DA start
                var req1 = new DisconnectReqMsg
                {
                    Type = neulib.MsgType.DADisconnectReq,
                };
                var buff1 = Serializer.Serialize<DisconnectReqMsg>(req1);
                subProcess.Request(in buff1, out byte[] result1);
                if (null != result1)
                {
                    var res1 = Serializer.Deserialize<DisconnectResMsg>(result1);
                }


                // UA start
                var req2 = new UAStopReqMsg
                {
                    Type = neulib.MsgType.UAStopReq
                };
                var buff2 = Serializer.Serialize<UAStopReqMsg>(req2);
                subProcess.Request(in buff2, out byte[] result2);
                if (null != result2)
                {
                    var res2 = Serializer.Deserialize<UAStopResMsg>(result2);
                }

                subProcess.SetDAArguments(DAHostComboBox.Text, DAServerComboBox.Text);
                subProcess.SetUAArguments(UAPortTextBox.Text, UAUserTextBox.Text, UAPasswordTextBox.Text);

                SwitchButton.Text = "Start";

                DAHostComboBox.Enabled = true;
                DAServerComboBox.Enabled = true;
                TestButton.Enabled = true;

                UAPortTextBox.Enabled = true;
                UAUserTextBox.Enabled = true;
                UAPasswordTextBox.Enabled = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var config = new Config
            {
                DAHost = DAHostComboBox.Text,
                DAServer = DAServerComboBox.Text,
                UAUrl = UAPortTextBox.Text,
                UAUser = UAUserTextBox.Text,
                UAPassword = UAPasswordTextBox.Text,
                AutoConnect = CheckBox.Checked
            };

            var _nodes = new List<string>();
            foreach (var item in dataItems)
            {
                _nodes.Add(item.ItemId);
            }

            config.Nodes = _nodes.ToArray();

            ConfigUtil.SaveConfig("neuopc.json", config);
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (1 == TabControl.SelectedIndex)
            {
                try
                {
                    Action action = () =>
                    {
                        MainListView.BeginUpdate();
                        MainListView.Items.Clear();
                        MainListView.EndUpdate();
                    };

                    Invoke(action);
                }
                catch (Exception exception)
                {
                    Log.Error($"clear list view error: {exception.Message}");
                }
            }

            if (2 == TabControl.SelectedIndex)
            {
                try
                {
                    Action action = () =>
                    {
                        DirectoryInfo di = new DirectoryInfo("./log");
                        LogListView.BeginUpdate();
                        LogListView.Items.Clear();

                        foreach (var fi in di.GetFiles())
                        {
                            ListViewItem lvi = new ListViewItem
                            {
                                Text = fi.Name,
                            };

                            lvi.SubItems.Add(fi.LastWriteTime.ToString());
                            lvi.SubItems.Add(fi.Length.ToString());
                            LogListView.Items.Add(lvi);
                        }

                        LogListView.EndUpdate();
                    };

                    Invoke(action);
                }
                catch (Exception exception)
                {
                    Log.Error($"get logs error: {exception.Message}");
                }
            }
        }

        private void LogListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewItem row = listView.GetItemAt(e.X, e.Y);
            ListViewItem.ListViewSubItem col = row.GetSubItemAt(e.X, e.Y);
            string strText = col.Text;
            try
            {
                Process.Start("notepad.exe", $"./log/{strText}");
            }
            catch (System.Exception ex)
            {
                Log.Error($"clipboard error:{ex.Message}");
            }
        }

        private void BtnAddNode_Click(object sender, EventArgs e)
        {
            if (null == treeViewDaBrowse.SelectedNode)
            {
                MessageBox.Show("Please select a node first.");
                return;
            }
            var name = treeViewDaBrowse.SelectedNode.Text;
            var itemId = treeViewDaBrowse.SelectedNode.Tag?.ToString();
            if (string.IsNullOrEmpty(itemId))
            {
                MessageBox.Show("Please select a node first.");
                return;
            }

            if (dataItems.Any(x => x.Name == name))
            {
                MessageBox.Show("Item already exists.");
                return;
            }
            dataItems.Add(new ViewModel(name, "", itemId));

        }

        private void BtnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 0) return;

            List<string> removes = new List<string>();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                if (!(row.DataBoundItem is ViewModel vm)) continue;
                removes.Add(vm.Name);
                dataItems.Remove(vm);
            }
        }

        private void BtnRemoveAll_Click(object sender, EventArgs e)
        {
            dataItems.Clear();
        }
    }
}
