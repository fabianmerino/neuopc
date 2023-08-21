
namespace neuopc
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            DAStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            UAStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            MainListView = new System.Windows.Forms.ListView();
            name = new System.Windows.Forms.ColumnHeader();
            type = new System.Windows.Forms.ColumnHeader();
            rights = new System.Windows.Forms.ColumnHeader();
            value = new System.Windows.Forms.ColumnHeader();
            quality = new System.Windows.Forms.ColumnHeader();
            error = new System.Windows.Forms.ColumnHeader();
            timestamp = new System.Windows.Forms.ColumnHeader();
            handle = new System.Windows.Forms.ColumnHeader();
            NotifyIcon = new System.Windows.Forms.NotifyIcon(components);
            TabControl = new System.Windows.Forms.TabControl();
            TabPageSetting = new System.Windows.Forms.TabPage();
            groupBox1 = new System.Windows.Forms.GroupBox();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            treeViewDaBrowse = new System.Windows.Forms.TreeView();
            btnRemoveAll = new System.Windows.Forms.Button();
            btnRemoveItem = new System.Windows.Forms.Button();
            btnAddNode = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            dataGridView1 = new System.Windows.Forms.DataGridView();
            Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            itemIdCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            SaveButton = new System.Windows.Forms.Button();
            UALabel = new System.Windows.Forms.Label();
            CheckBox = new System.Windows.Forms.CheckBox();
            SwitchButton = new System.Windows.Forms.Button();
            UAPasswordTextBox = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            UAPortTextBox = new System.Windows.Forms.TextBox();
            UAUserTextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            DAHostComboBox = new System.Windows.Forms.ComboBox();
            DAServerComboBox = new System.Windows.Forms.ComboBox();
            label2 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            TestButton = new System.Windows.Forms.Button();
            TabPageData = new System.Windows.Forms.TabPage();
            TabPageLog = new System.Windows.Forms.TabPage();
            LogListView = new System.Windows.Forms.ListView();
            filename = new System.Windows.Forms.ColumnHeader();
            time = new System.Windows.Forms.ColumnHeader();
            length = new System.Windows.Forms.ColumnHeader();
            TabPageAbout = new System.Windows.Forms.TabPage();
            statusStrip1.SuspendLayout();
            TabControl.SuspendLayout();
            TabPageSetting.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            TabPageData.SuspendLayout();
            TabPageLog.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { DAStatusLabel, UAStatusLabel });
            statusStrip1.Location = new System.Drawing.Point(0, 704);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            statusStrip1.Size = new System.Drawing.Size(1114, 22);
            statusStrip1.TabIndex = 4;
            statusStrip1.Text = "statusStrip1";
            // 
            // DAStatusLabel
            // 
            DAStatusLabel.Name = "DAStatusLabel";
            DAStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // UAStatusLabel
            // 
            UAStatusLabel.Name = "UAStatusLabel";
            UAStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainListView
            // 
            MainListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { name, type, rights, value, quality, error, timestamp, handle });
            MainListView.Dock = System.Windows.Forms.DockStyle.Fill;
            MainListView.HideSelection = false;
            MainListView.Location = new System.Drawing.Point(2, 2);
            MainListView.Margin = new System.Windows.Forms.Padding(2);
            MainListView.Name = "MainListView";
            MainListView.Size = new System.Drawing.Size(1102, 670);
            MainListView.TabIndex = 7;
            MainListView.UseCompatibleStateImageBehavior = false;
            MainListView.View = System.Windows.Forms.View.Details;
            MainListView.MouseDoubleClick += MainListView_MouseDoubleClick;
            // 
            // name
            // 
            name.Text = "Name";
            name.Width = 250;
            // 
            // type
            // 
            type.Text = "Type";
            type.Width = 100;
            // 
            // rights
            // 
            rights.Text = "Rights";
            rights.Width = 100;
            // 
            // value
            // 
            value.Text = "Value";
            value.Width = 250;
            // 
            // quality
            // 
            quality.Text = "Quality";
            quality.Width = 80;
            // 
            // error
            // 
            error.Text = "Error";
            error.Width = 80;
            // 
            // timestamp
            // 
            timestamp.Text = "Timestamp";
            timestamp.Width = 200;
            // 
            // handle
            // 
            handle.Text = "Index";
            handle.Width = 0;
            // 
            // NotifyIcon
            // 
            NotifyIcon.Text = "neuopc";
            NotifyIcon.Visible = true;
            NotifyIcon.Click += NotifyIcon_Click;
            // 
            // TabControl
            // 
            TabControl.Controls.Add(TabPageSetting);
            TabControl.Controls.Add(TabPageData);
            TabControl.Controls.Add(TabPageLog);
            TabControl.Controls.Add(TabPageAbout);
            TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            TabControl.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            TabControl.Location = new System.Drawing.Point(0, 0);
            TabControl.Margin = new System.Windows.Forms.Padding(2);
            TabControl.Name = "TabControl";
            TabControl.SelectedIndex = 0;
            TabControl.Size = new System.Drawing.Size(1114, 704);
            TabControl.TabIndex = 8;
            TabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            // 
            // TabPageSetting
            // 
            TabPageSetting.Controls.Add(groupBox1);
            TabPageSetting.Location = new System.Drawing.Point(4, 26);
            TabPageSetting.Margin = new System.Windows.Forms.Padding(2);
            TabPageSetting.Name = "TabPageSetting";
            TabPageSetting.Padding = new System.Windows.Forms.Padding(2);
            TabPageSetting.Size = new System.Drawing.Size(1106, 674);
            TabPageSetting.TabIndex = 0;
            TabPageSetting.Text = "Setting";
            TabPageSetting.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(splitContainer1);
            groupBox1.Controls.Add(SaveButton);
            groupBox1.Controls.Add(UALabel);
            groupBox1.Controls.Add(CheckBox);
            groupBox1.Controls.Add(SwitchButton);
            groupBox1.Controls.Add(UAPasswordTextBox);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(UAPortTextBox);
            groupBox1.Controls.Add(UAUserTextBox);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(DAHostComboBox);
            groupBox1.Controls.Add(DAServerComboBox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(TestButton);
            groupBox1.Location = new System.Drawing.Point(6, 4);
            groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            groupBox1.Size = new System.Drawing.Size(1097, 673);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new System.Drawing.Point(6, 122);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(treeViewDaBrowse);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(btnRemoveAll);
            splitContainer1.Panel2.Controls.Add(btnRemoveItem);
            splitContainer1.Panel2.Controls.Add(btnAddNode);
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Size = new System.Drawing.Size(1085, 538);
            splitContainer1.SplitterDistance = 361;
            splitContainer1.TabIndex = 36;
            // 
            // treeViewDaBrowse
            // 
            treeViewDaBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            treeViewDaBrowse.Location = new System.Drawing.Point(0, 0);
            treeViewDaBrowse.Name = "treeViewDaBrowse";
            treeViewDaBrowse.ShowRootLines = false;
            treeViewDaBrowse.Size = new System.Drawing.Size(361, 538);
            treeViewDaBrowse.TabIndex = 0;
            treeViewDaBrowse.AfterCollapse += TreeViewDABrowse_AfterCollapse;
            treeViewDaBrowse.AfterExpand += TreeViewDABrowse_AfterExpand;
            treeViewDaBrowse.DoubleClick += TreeViewDABrowse_DoubleClick;
            // 
            // btnRemoveAll
            // 
            btnRemoveAll.Location = new System.Drawing.Point(2, 275);
            btnRemoveAll.Name = "btnRemoveAll";
            btnRemoveAll.Size = new System.Drawing.Size(125, 23);
            btnRemoveAll.TabIndex = 3;
            btnRemoveAll.Text = "<< Remove All";
            btnRemoveAll.UseVisualStyleBackColor = true;
            btnRemoveAll.Click += BtnRemoveAll_Click;
            // 
            // btnRemoveItem
            // 
            btnRemoveItem.Location = new System.Drawing.Point(3, 246);
            btnRemoveItem.Name = "btnRemoveItem";
            btnRemoveItem.Size = new System.Drawing.Size(125, 23);
            btnRemoveItem.TabIndex = 3;
            btnRemoveItem.Text = "<< Remove Item";
            btnRemoveItem.UseVisualStyleBackColor = true;
            btnRemoveItem.Click += BtnRemoveItem_Click;
            // 
            // btnAddNode
            // 
            btnAddNode.Location = new System.Drawing.Point(3, 217);
            btnAddNode.Name = "btnAddNode";
            btnAddNode.Size = new System.Drawing.Size(125, 23);
            btnAddNode.TabIndex = 2;
            btnAddNode.Text = "Add node >>";
            btnAddNode.UseVisualStyleBackColor = true;
            btnAddNode.Click += BtnAddNode_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(dataGridView1);
            panel1.Location = new System.Drawing.Point(134, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(583, 532);
            panel1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column1, Column2, itemIdCol });
            dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridView1.Location = new System.Drawing.Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new System.Drawing.Size(583, 532);
            dataGridView1.TabIndex = 0;
            dataGridView1.RowStateChanged += DataGridView1_RowStateChanged;
            // 
            // Column1
            // 
            Column1.DataPropertyName = "Name";
            Column1.HeaderText = "Name";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            // 
            // Column2
            // 
            Column2.DataPropertyName = "Type";
            Column2.HeaderText = "Type";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            // 
            // itemIdCol
            // 
            itemIdCol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            itemIdCol.DataPropertyName = "ItemID";
            itemIdCol.HeaderText = "Item ID";
            itemIdCol.Name = "itemIdCol";
            itemIdCol.ReadOnly = true;
            // 
            // SaveButton
            // 
            SaveButton.Location = new System.Drawing.Point(988, 93);
            SaveButton.Margin = new System.Windows.Forms.Padding(2);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new System.Drawing.Size(102, 21);
            SaveButton.TabIndex = 35;
            SaveButton.Text = "Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // UALabel
            // 
            UALabel.AutoSize = true;
            UALabel.Location = new System.Drawing.Point(144, 75);
            UALabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            UALabel.Name = "UALabel";
            UALabel.Size = new System.Drawing.Size(0, 17);
            UALabel.TabIndex = 34;
            // 
            // CheckBox
            // 
            CheckBox.AutoSize = true;
            CheckBox.Location = new System.Drawing.Point(762, 96);
            CheckBox.Margin = new System.Windows.Forms.Padding(2);
            CheckBox.Name = "CheckBox";
            CheckBox.Size = new System.Drawing.Size(121, 21);
            CheckBox.TabIndex = 33;
            CheckBox.Text = "Auto connection";
            CheckBox.UseVisualStyleBackColor = true;
            // 
            // SwitchButton
            // 
            SwitchButton.Location = new System.Drawing.Point(879, 93);
            SwitchButton.Margin = new System.Windows.Forms.Padding(2);
            SwitchButton.Name = "SwitchButton";
            SwitchButton.Size = new System.Drawing.Size(102, 21);
            SwitchButton.TabIndex = 32;
            SwitchButton.Text = "Start";
            SwitchButton.UseVisualStyleBackColor = true;
            SwitchButton.Click += SwitchButton_Click;
            // 
            // UAPasswordTextBox
            // 
            UAPasswordTextBox.Location = new System.Drawing.Point(773, 65);
            UAPasswordTextBox.Margin = new System.Windows.Forms.Padding(2);
            UAPasswordTextBox.Name = "UAPasswordTextBox";
            UAPasswordTextBox.Size = new System.Drawing.Size(318, 23);
            UAPasswordTextBox.TabIndex = 29;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(634, 69);
            label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(113, 17);
            label5.TabIndex = 30;
            label5.Text = "OPCUA Password:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(634, 14);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(81, 17);
            label3.TabIndex = 27;
            label3.Text = "OPCUA Port:";
            // 
            // UAPortTextBox
            // 
            UAPortTextBox.Location = new System.Drawing.Point(773, 14);
            UAPortTextBox.Margin = new System.Windows.Forms.Padding(1);
            UAPortTextBox.Name = "UAPortTextBox";
            UAPortTextBox.Size = new System.Drawing.Size(318, 23);
            UAPortTextBox.TabIndex = 25;
            // 
            // UAUserTextBox
            // 
            UAUserTextBox.Location = new System.Drawing.Point(773, 40);
            UAUserTextBox.Margin = new System.Windows.Forms.Padding(2);
            UAUserTextBox.Name = "UAUserTextBox";
            UAUserTextBox.Size = new System.Drawing.Size(318, 23);
            UAUserTextBox.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(634, 42);
            label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(84, 17);
            label4.TabIndex = 28;
            label4.Text = "OPCUA User:";
            // 
            // DAHostComboBox
            // 
            DAHostComboBox.FormattingEnabled = true;
            DAHostComboBox.Location = new System.Drawing.Point(144, 14);
            DAHostComboBox.Margin = new System.Windows.Forms.Padding(2);
            DAHostComboBox.Name = "DAHostComboBox";
            DAHostComboBox.Size = new System.Drawing.Size(318, 25);
            DAHostComboBox.TabIndex = 0;
            DAHostComboBox.DropDown += DAHostComboBox_DropDown;
            // 
            // DAServerComboBox
            // 
            DAServerComboBox.FormattingEnabled = true;
            DAServerComboBox.Location = new System.Drawing.Point(144, 41);
            DAServerComboBox.Margin = new System.Windows.Forms.Padding(2);
            DAServerComboBox.Name = "DAServerComboBox";
            DAServerComboBox.Size = new System.Drawing.Size(318, 25);
            DAServerComboBox.TabIndex = 1;
            DAServerComboBox.DropDown += DAServerComboBox_DropDown;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(5, 43);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(94, 17);
            label2.TabIndex = 6;
            label2.Text = "OPCDA Server:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(5, 17);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(84, 17);
            label1.TabIndex = 3;
            label1.Text = "OPCDA Host:";
            // 
            // TestButton
            // 
            TestButton.Location = new System.Drawing.Point(359, 72);
            TestButton.Margin = new System.Windows.Forms.Padding(1);
            TestButton.Name = "TestButton";
            TestButton.Size = new System.Drawing.Size(102, 21);
            TestButton.TabIndex = 2;
            TestButton.Text = "Connection Test";
            TestButton.UseVisualStyleBackColor = true;
            TestButton.Click += TestButton_Click;
            // 
            // TabPageData
            // 
            TabPageData.Controls.Add(MainListView);
            TabPageData.Location = new System.Drawing.Point(4, 26);
            TabPageData.Margin = new System.Windows.Forms.Padding(2);
            TabPageData.Name = "TabPageData";
            TabPageData.Padding = new System.Windows.Forms.Padding(2);
            TabPageData.Size = new System.Drawing.Size(1106, 674);
            TabPageData.TabIndex = 1;
            TabPageData.Text = "Data view";
            TabPageData.UseVisualStyleBackColor = true;
            // 
            // TabPageLog
            // 
            TabPageLog.Controls.Add(LogListView);
            TabPageLog.Location = new System.Drawing.Point(4, 26);
            TabPageLog.Margin = new System.Windows.Forms.Padding(2);
            TabPageLog.Name = "TabPageLog";
            TabPageLog.Size = new System.Drawing.Size(1106, 674);
            TabPageLog.TabIndex = 3;
            TabPageLog.Text = "Log";
            TabPageLog.UseVisualStyleBackColor = true;
            // 
            // LogListView
            // 
            LogListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { filename, time, length });
            LogListView.Dock = System.Windows.Forms.DockStyle.Fill;
            LogListView.HideSelection = false;
            LogListView.Location = new System.Drawing.Point(0, 0);
            LogListView.Margin = new System.Windows.Forms.Padding(2);
            LogListView.Name = "LogListView";
            LogListView.Size = new System.Drawing.Size(1106, 674);
            LogListView.TabIndex = 0;
            LogListView.UseCompatibleStateImageBehavior = false;
            LogListView.View = System.Windows.Forms.View.Details;
            LogListView.MouseDoubleClick += LogListView_MouseDoubleClick;
            // 
            // filename
            // 
            filename.Text = "Name";
            filename.Width = 300;
            // 
            // time
            // 
            time.Text = "Time";
            time.Width = 200;
            // 
            // length
            // 
            length.Text = "Length";
            length.Width = 200;
            // 
            // TabPageAbout
            // 
            TabPageAbout.Location = new System.Drawing.Point(4, 26);
            TabPageAbout.Margin = new System.Windows.Forms.Padding(2);
            TabPageAbout.Name = "TabPageAbout";
            TabPageAbout.Padding = new System.Windows.Forms.Padding(2);
            TabPageAbout.Size = new System.Drawing.Size(1106, 674);
            TabPageAbout.TabIndex = 2;
            TabPageAbout.Text = "About";
            TabPageAbout.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1114, 726);
            Controls.Add(TabControl);
            Controls.Add(statusStrip1);
            Margin = new System.Windows.Forms.Padding(1);
            Name = "MainForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "NeuOPC";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            TabControl.ResumeLayout(false);
            TabPageSetting.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            TabPageData.ResumeLayout(false);
            TabPageLog.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel DAStatusLabel;
        private System.Windows.Forms.ListView MainListView;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.ColumnHeader handle;
        private System.Windows.Forms.ColumnHeader name;
        private System.Windows.Forms.ColumnHeader quality;
        private System.Windows.Forms.ColumnHeader value;
        private System.Windows.Forms.ColumnHeader rights;
        private System.Windows.Forms.ColumnHeader error;
        private System.Windows.Forms.ColumnHeader timestamp;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ToolStripStatusLabel UAStatusLabel;
        private System.Windows.Forms.TabControl TabControl;
        private System.Windows.Forms.TabPage TabPageSetting;
        private System.Windows.Forms.TabPage TabPageData;
        private System.Windows.Forms.TabPage TabPageAbout;
        private System.Windows.Forms.TabPage TabPageWriteLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button SwitchButton;
        private System.Windows.Forms.TextBox UAPasswordTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UAPortTextBox;
        private System.Windows.Forms.TextBox UAUserTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox DAHostComboBox;
        private System.Windows.Forms.ComboBox DAServerComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.CheckBox CheckBox;
        private System.Windows.Forms.Label UALabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TabPage TabPageLog;
        private System.Windows.Forms.ListView LogListView;
        private System.Windows.Forms.ColumnHeader filename;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.ColumnHeader length;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeViewDaBrowse;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnRemoveAll;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnAddNode;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemIdCol;
    }
}

