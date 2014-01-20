namespace PROFTESTCLIENT
{
    partial class clientview
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.datagridReg = new System.Windows.Forms.DataGridView();
            this.client = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilesLocationGrid = new System.Windows.Forms.DataGridView();
            this.list_IP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.list_Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_register = new System.Windows.Forms.Button();
            this.button_unregister = new System.Windows.Forms.Button();
            this.FilesList = new System.Windows.Forms.DataGridView();
            this.FileNames = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_ListFiles = new System.Windows.Forms.Button();
            this.button_ListLocations = new System.Windows.Forms.Button();
            this.lb_status_text = new System.Windows.Forms.Label();
            this.lb_status = new System.Windows.Forms.Label();
            this.button_connect = new System.Windows.Forms.Button();
            this.lb_connect = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.datagridReg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesLocationGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesList)).BeginInit();
            this.SuspendLayout();
            // 
            // datagridReg
            // 
            this.datagridReg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datagridReg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.client,
            this.IP,
            this.Port});
            this.datagridReg.Location = new System.Drawing.Point(1, 26);
            this.datagridReg.Name = "datagridReg";
            this.datagridReg.Size = new System.Drawing.Size(345, 601);
            this.datagridReg.TabIndex = 0;
            // 
            // client
            // 
            this.client.HeaderText = "File Name";
            this.client.Name = "client";
            // 
            // IP
            // 
            this.IP.HeaderText = "IP";
            this.IP.Name = "IP";
            // 
            // Port
            // 
            this.Port.HeaderText = "Port";
            this.Port.Name = "Port";
            // 
            // FilesLocationGrid
            // 
            this.FilesLocationGrid.AllowUserToAddRows = false;
            this.FilesLocationGrid.AllowUserToDeleteRows = false;
            this.FilesLocationGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FilesLocationGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.list_IP,
            this.list_Port});
            this.FilesLocationGrid.Location = new System.Drawing.Point(611, 26);
            this.FilesLocationGrid.Name = "FilesLocationGrid";
            this.FilesLocationGrid.Size = new System.Drawing.Size(245, 601);
            this.FilesLocationGrid.TabIndex = 1;
            this.FilesLocationGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // list_IP
            // 
            this.list_IP.HeaderText = "IP";
            this.list_IP.Name = "list_IP";
            // 
            // list_Port
            // 
            this.list_Port.HeaderText = "Port";
            this.list_Port.Name = "list_Port";
            // 
            // button_register
            // 
            this.button_register.Location = new System.Drawing.Point(1, 633);
            this.button_register.Name = "button_register";
            this.button_register.Size = new System.Drawing.Size(75, 23);
            this.button_register.TabIndex = 2;
            this.button_register.Text = "Register";
            this.button_register.UseVisualStyleBackColor = true;
            this.button_register.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_unregister
            // 
            this.button_unregister.Location = new System.Drawing.Point(271, 633);
            this.button_unregister.Name = "button_unregister";
            this.button_unregister.Size = new System.Drawing.Size(75, 23);
            this.button_unregister.TabIndex = 3;
            this.button_unregister.Text = "Unregister";
            this.button_unregister.UseVisualStyleBackColor = true;
            this.button_unregister.Click += new System.EventHandler(this.button_unregister_Click);
            // 
            // FilesList
            // 
            this.FilesList.AllowUserToAddRows = false;
            this.FilesList.AllowUserToDeleteRows = false;
            this.FilesList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FilesList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileNames});
            this.FilesList.Location = new System.Drawing.Point(386, 26);
            this.FilesList.MultiSelect = false;
            this.FilesList.Name = "FilesList";
            this.FilesList.ReadOnly = true;
            this.FilesList.Size = new System.Drawing.Size(219, 601);
            this.FilesList.TabIndex = 4;
            // 
            // FileNames
            // 
            this.FileNames.HeaderText = "File Name";
            this.FileNames.Name = "FileNames";
            this.FileNames.ReadOnly = true;
            // 
            // button_ListFiles
            // 
            this.button_ListFiles.Location = new System.Drawing.Point(386, 633);
            this.button_ListFiles.Name = "button_ListFiles";
            this.button_ListFiles.Size = new System.Drawing.Size(75, 23);
            this.button_ListFiles.TabIndex = 5;
            this.button_ListFiles.Text = "List Files";
            this.button_ListFiles.UseVisualStyleBackColor = true;
            this.button_ListFiles.Click += new System.EventHandler(this.button_ListFiles_Click);
            // 
            // button_ListLocations
            // 
            this.button_ListLocations.Location = new System.Drawing.Point(611, 633);
            this.button_ListLocations.Name = "button_ListLocations";
            this.button_ListLocations.Size = new System.Drawing.Size(120, 23);
            this.button_ListLocations.TabIndex = 6;
            this.button_ListLocations.Text = "List Locations";
            this.button_ListLocations.UseVisualStyleBackColor = true;
            this.button_ListLocations.Click += new System.EventHandler(this.button_ListLocations_Click);
            // 
            // lb_status_text
            // 
            this.lb_status_text.AutoSize = true;
            this.lb_status_text.Location = new System.Drawing.Point(352, 9);
            this.lb_status_text.Name = "lb_status_text";
            this.lb_status_text.Size = new System.Drawing.Size(0, 13);
            this.lb_status_text.TabIndex = 7;
            // 
            // lb_status
            // 
            this.lb_status.AutoSize = true;
            this.lb_status.Location = new System.Drawing.Point(310, 9);
            this.lb_status.Name = "lb_status";
            this.lb_status.Size = new System.Drawing.Size(40, 13);
            this.lb_status.TabIndex = 8;
            this.lb_status.Text = "Status:";
            this.lb_status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(1, 4);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 9;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // lb_connect
            // 
            this.lb_connect.AutoSize = true;
            this.lb_connect.Location = new System.Drawing.Point(806, 9);
            this.lb_connect.Name = "lb_connect";
            this.lb_connect.Size = new System.Drawing.Size(73, 13);
            this.lb_connect.TabIndex = 10;
            this.lb_connect.Text = "Disconnected";
            // 
            // clientview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(868, 668);
            this.Controls.Add(this.lb_connect);
            this.Controls.Add(this.button_connect);
            this.Controls.Add(this.lb_status);
            this.Controls.Add(this.lb_status_text);
            this.Controls.Add(this.button_ListLocations);
            this.Controls.Add(this.button_ListFiles);
            this.Controls.Add(this.FilesList);
            this.Controls.Add(this.button_unregister);
            this.Controls.Add(this.button_register);
            this.Controls.Add(this.FilesLocationGrid);
            this.Controls.Add(this.datagridReg);
            this.Text = "Client Control";
            ((System.ComponentModel.ISupportInitialize)(this.datagridReg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesLocationGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FilesList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView datagridReg;
        private System.Windows.Forms.DataGridView FilesLocationGrid;
        private System.Windows.Forms.Button button_register;
        private System.Windows.Forms.Button button_unregister;
        private System.Windows.Forms.DataGridView FilesList;
        private System.Windows.Forms.DataGridViewTextBoxColumn list_IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn list_Port;
        private System.Windows.Forms.Button button_ListFiles;
        private System.Windows.Forms.Button button_ListLocations;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn IP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Port;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileNames;
        private System.Windows.Forms.Label lb_status_text;
        private System.Windows.Forms.Label lb_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn client;
        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.Label lb_connect;
    }
}