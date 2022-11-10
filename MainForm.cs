﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OPCAutomation;

namespace neuopc
{
    public partial class MainForm : Form
    {
        private DAClient client;
        private UAServer server;
        private List<Item> items;

        public MainForm(DAClient client, UAServer server) : this()
        {
            this.client = client;
            this.server = server;
        }

        public MainForm()
        {
            InitializeComponent();
        }

        public void UpdateListView(List<Item> list)
        {
            foreach (var i in list)
            {
                Action<Item> action = (data) =>
                {
                    int index = data.ClientHandle;
                    var items = MainListView.Items;
                    var item = items[index];
                    var subItemValue = item.SubItems[4];
                    var subItemQuality = item.SubItems[5];
                    var subItemError = item.SubItems[6];
                    var subItemTs = item.SubItems[7];

                    subItemValue.Text = Convert.ToString(data.Value);
                    subItemQuality.Text = data.Quality.ToString();
                    subItemError.Text = data.Error.ToString();
                    subItemTs.Text = Convert.ToString(data.Timestamp);
                };

                Invoke(action, i);
            }
        }


        private void ReadButton_Click(object sender, EventArgs e)
        {
            client.Connect(DAHostComboBox.Text, DAServerComboBox.Text);
            items = client.BuildGroup();
            MainListView.BeginUpdate();
            MainListView.Items.Clear();
            for (int i = 0; i < items.Count; i++)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = items[i].ClientHandle.ToString(); // handle
                lvi.SubItems.Add(items[i].Name.ToString()); // name
                lvi.SubItems.Add(items[i].Type.ToString()); // type
                lvi.SubItems.Add(items[i].Rights.ToString()); // rights
                lvi.SubItems.Add(""); // value
                lvi.SubItems.Add(""); // quality
                lvi.SubItems.Add(""); // error
                lvi.SubItems.Add(""); // timestamp
                MainListView.Items.Add(lvi);
            }
            MainListView.EndUpdate();

            client.Update += UpdateListView;
            client.Read();
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            UAPortTextBox.Text = "48401";
        }

        private void DAServerComboBox_DropDown(object sender, EventArgs e)
        {
            DAServerComboBox.Text = string.Empty;
            DAServerComboBox.Items.Clear();
            var list = client.GetServers(DAHostComboBox.Text);
            DAServerComboBox.Items.AddRange(list.ToArray());
            if (0 < DAServerComboBox.Items.Count)
            {
                DAServerComboBox.SelectedIndex = 0;
            }
        }

        private void DAHostComboBox_DropDown(object sender, EventArgs e)
        {
            DAHostComboBox.Text = string.Empty;
            DAHostComboBox.Items.Clear();
            var list = client.GetHosts();
            DAHostComboBox.Items.AddRange(list.ToArray());
            if (0 < DAHostComboBox.Items.Count)
            {
                DAHostComboBox.SelectedIndex = 0;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Stop();
            server.Stop();
            NotifyIcon.Dispose();
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            server.Write += client.Write;
            server.Start(UAPortTextBox.Text, items);
            client.Update += server.UpdateNodes;
        }
    }
}
