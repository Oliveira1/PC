using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFilesSearch
{
    public partial class FileSearch : Form
    {
        private delegate void SetTextCallback(string text);

        private delegate void SetListViewCallBack(ListView lv);

        public FileSearch()
        {
            InitializeComponent();
        }
        
        private void button_path_Click(object sender, EventArgs e)
        {
            folderBrowser.ShowDialog();
            lb_path.Text = folderBrowser.SelectedPath;
        }

        private  void button_search_Click(object sender, EventArgs e)
        {
            if (text_char_sequence.Text.Equals("") || text_extension.Text.Equals("") || lb_path.Text.Equals(""))
            {
                lb_search.Text = "Not enough arguments for Search";
                return;
            }
            button_search.Enabled = false;
            String dir = lb_path.Text;
            String ext = text_extension.Text;
            String char_seq = text_char_sequence.Text;
            lb_found_value.Text = "0";
            lb_total_value.Text = "0";
            lb_search.Text = "Searching...";
            DirectoryInfo di = new DirectoryInfo(dir);
            cts = new CancellationTokenSource();
            Task.Factory.StartNew(() =>
            {
                ListView lv = new ListView();
                IEnumerable<Exercicio3.SearchResult> results = Exercicio3.Find_By_Sequence(di, ext, char_seq, cts);
                foreach (var result in results)
                {
                    if (result == null) break;
                    lb_found_SetText(Convert.ToString(result.matching_sequence + Convert.ToInt32(lb_found_value.Text)));
                    lb_total_SetText(Convert.ToString(result.total + Convert.ToInt32(lb_total_value.Text)));
                    foreach (var row in result.paths)
                    {
                        var item = new ListViewItem(row);
                        lv.Items.Add(item);
                    }
                }
                    Invoke((MethodInvoker)delegate { lb_search.Text = "Preparing Results..."; });
                     SetListView(lv);
                Invoke((MethodInvoker) delegate { lb_search.Text = "Done"; });
            if (cts.Token.IsCancellationRequested)
                Invoke((MethodInvoker)delegate { lb_search.Text = "Search Incomplete: Canceled"; });
                Invoke((MethodInvoker) delegate { button_search.Enabled = true; });
            }, cts.Token);
        }


        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (cts != null) { 
                cts.Cancel();
            Invoke((MethodInvoker)delegate { lb_search.Text = "Preparing Results..."; });
            }
        }


        private void SetListView(ListView lv)
        {
            if (list_paths.InvokeRequired)
            {
                SetListViewCallBack d = new SetListViewCallBack(SetListView);
                Invoke(d, new object[] {lv});
            }
            else
            {
                foreach (ListViewItem item in lv.Items)
                {
                    lv.Items.Remove(item);
                    list_paths.Items.Add(item);
                }
                list_paths.Update();
            }
        }

        private void lb_found_SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (lb_found_value.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(lb_found_SetText);
                this.Invoke(d, new object[] {text});
            }
            else
            {
                lb_found_value.Text = text;
                lb_found_value.Refresh();
            }
        }

        private void lb_total_SetText(string text)
        {
            if (this.lb_found_value.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(lb_total_SetText);
                this.Invoke(d, new object[] {text});
            }
            else
            {
                this.lb_total_value.Text = text;
                this.lb_total_value.Refresh();
            }
        }
    }
}