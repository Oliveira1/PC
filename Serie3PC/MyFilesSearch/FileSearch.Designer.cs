namespace MyFilesSearch
{
    partial class FileSearch
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
            this.button_search = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.lb_found = new System.Windows.Forms.Label();
            this.lb_total = new System.Windows.Forms.Label();
            this.lb_found_value = new System.Windows.Forms.Label();
            this.lb_total_value = new System.Windows.Forms.Label();
            this.list_paths = new System.Windows.Forms.ListView();
            this.text_extension = new System.Windows.Forms.TextBox();
            this.text_char_sequence = new System.Windows.Forms.TextBox();
            this.lb_char_sequence = new System.Windows.Forms.Label();
            this.lb_extension = new System.Windows.Forms.Label();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.lb_path = new System.Windows.Forms.Label();
            this.button_path = new System.Windows.Forms.Button();
            this.lb_search = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(854, 61);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(75, 23);
            this.button_search.TabIndex = 0;
            this.button_search.Text = "Search";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(854, 90);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 1;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // lb_found
            // 
            this.lb_found.AutoSize = true;
            this.lb_found.Location = new System.Drawing.Point(345, 651);
            this.lb_found.Name = "lb_found";
            this.lb_found.Size = new System.Drawing.Size(40, 13);
            this.lb_found.TabIndex = 3;
            this.lb_found.Text = "found :";
            // 
            // lb_total
            // 
            this.lb_total.AutoSize = true;
            this.lb_total.Location = new System.Drawing.Point(605, 651);
            this.lb_total.Name = "lb_total";
            this.lb_total.Size = new System.Drawing.Size(37, 13);
            this.lb_total.TabIndex = 4;
            this.lb_total.Text = "Total :";
            // 
            // lb_found_value
            // 
            this.lb_found_value.AutoSize = true;
            this.lb_found_value.Location = new System.Drawing.Point(400, 651);
            this.lb_found_value.Name = "lb_found_value";
            this.lb_found_value.Size = new System.Drawing.Size(13, 13);
            this.lb_found_value.TabIndex = 5;
            this.lb_found_value.Text = "0";
            // 
            // lb_total_value
            // 
            this.lb_total_value.AutoSize = true;
            this.lb_total_value.Location = new System.Drawing.Point(661, 650);
            this.lb_total_value.Name = "lb_total_value";
            this.lb_total_value.Size = new System.Drawing.Size(13, 13);
            this.lb_total_value.TabIndex = 6;
            this.lb_total_value.Text = "0";
            // 
            // list_paths
            // 
            this.list_paths.Location = new System.Drawing.Point(3, 130);
            this.list_paths.Name = "list_paths";
            this.list_paths.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.list_paths.Size = new System.Drawing.Size(926, 503);
            this.list_paths.TabIndex = 7;
            this.list_paths.UseCompatibleStateImageBehavior = false;
            this.list_paths.View = System.Windows.Forms.View.List;
            // 
            // text_extension
            // 
            this.text_extension.Location = new System.Drawing.Point(481, 64);
            this.text_extension.Name = "text_extension";
            this.text_extension.Size = new System.Drawing.Size(100, 20);
            this.text_extension.TabIndex = 9;
            // 
            // text_char_sequence
            // 
            this.text_char_sequence.Location = new System.Drawing.Point(715, 64);
            this.text_char_sequence.Name = "text_char_sequence";
            this.text_char_sequence.Size = new System.Drawing.Size(80, 20);
            this.text_char_sequence.TabIndex = 10;
            this.text_char_sequence.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // lb_char_sequence
            // 
            this.lb_char_sequence.AutoSize = true;
            this.lb_char_sequence.Location = new System.Drawing.Point(655, 70);
            this.lb_char_sequence.Name = "lb_char_sequence";
            this.lb_char_sequence.Size = new System.Drawing.Size(56, 13);
            this.lb_char_sequence.TabIndex = 11;
            this.lb_char_sequence.Text = "Sequence";
            // 
            // lb_extension
            // 
            this.lb_extension.AutoSize = true;
            this.lb_extension.Location = new System.Drawing.Point(422, 70);
            this.lb_extension.Name = "lb_extension";
            this.lb_extension.Size = new System.Drawing.Size(53, 13);
            this.lb_extension.TabIndex = 12;
            this.lb_extension.Text = "Extension";
            // 
            // lb_path
            // 
            this.lb_path.AutoSize = true;
            this.lb_path.Location = new System.Drawing.Point(12, 114);
            this.lb_path.Name = "lb_path";
            this.lb_path.Size = new System.Drawing.Size(29, 13);
            this.lb_path.TabIndex = 13;
            this.lb_path.Text = "Path";
            // 
            // button_path
            // 
            this.button_path.Location = new System.Drawing.Point(107, 62);
            this.button_path.Name = "button_path";
            this.button_path.Size = new System.Drawing.Size(75, 23);
            this.button_path.TabIndex = 14;
            this.button_path.Text = "Path...";
            this.button_path.UseVisualStyleBackColor = true;
            this.button_path.Click += new System.EventHandler(this.button_path_Click);
            // 
            // lb_search
            // 
            this.lb_search.AutoSize = true;
            this.lb_search.Location = new System.Drawing.Point(22, 651);
            this.lb_search.Name = "lb_search";
            this.lb_search.Size = new System.Drawing.Size(0, 13);
            this.lb_search.TabIndex = 15;
            // 
            // FileSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1080, 686);
            this.Controls.Add(this.lb_search);
            this.Controls.Add(this.button_path);
            this.Controls.Add(this.lb_path);
            this.Controls.Add(this.lb_extension);
            this.Controls.Add(this.lb_char_sequence);
            this.Controls.Add(this.text_char_sequence);
            this.Controls.Add(this.text_extension);
            this.Controls.Add(this.list_paths);
            this.Controls.Add(this.lb_total_value);
            this.Controls.Add(this.lb_found_value);
            this.Controls.Add(this.lb_total);
            this.Controls.Add(this.lb_found);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_search);
            this.Name = "FileSearch";
            this.Text = "FileSearch";
            this.Load += new System.EventHandler(this.FileSearch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.Label lb_found;
        private System.Windows.Forms.Label lb_total;
        private System.Windows.Forms.Label lb_found_value;
        private System.Windows.Forms.Label lb_total_value;
        private System.Windows.Forms.ListView list_paths;
        private System.Windows.Forms.TextBox text_extension;
        private System.Windows.Forms.TextBox text_char_sequence;
        private System.Windows.Forms.Label lb_char_sequence;
        private System.Windows.Forms.Label lb_extension;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.Label lb_path;
        private System.Windows.Forms.Button button_path;
        private System.Windows.Forms.Label lb_search;
    }
}