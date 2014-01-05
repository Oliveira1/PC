using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyFilesSearch
{
    public partial class FileSearch : Form
    {
        public FileSearch()
        {
            InitializeComponent();
        }
        private void FileSearch_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void text_path_TextChanged(object sender, EventArgs e)
        {
            folderBrowser=new FolderBrowserDialog();
            Console.WriteLine(folderBrowser.SelectedPath);
            
        }

        private void button_path_Click(object sender, EventArgs e)
        {
           
            folderBrowser.ShowDialog();
            lb_path.Text = folderBrowser.SelectedPath;
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            String dir = lb_path.Text;
            String ext = text_extension.Text;
            String char_seq=text_char_sequence.Text;
            lb_found_value.Text = "0";
            lb_total_value.Text = "0";
            lb_search.Text = "Searching...";
            DirectoryInfo di=new DirectoryInfo(dir);
           IEnumerable<Exercicio3.SearchResult> results= Exercicio3.Find_By_Sequence(di, ext, char_seq);

            foreach (var result in results)
            {
                lb_found_value.Text =Convert.ToString(result.matching_sequence+Convert.ToInt32(lb_found_value.Text));
                lb_total_value.Text = Convert.ToString(result.total+ Convert.ToInt32(lb_total_value.Text));
                foreach (var row in result.paths)
                {
                    var item = new ListViewItem(row);
                    list_paths.Items.Add(item);
                }
                list_paths.Update();
                lb_found_value.Update();
                lb_total_value.Update();
            }
            lb_search.Text = "Done";
        }

    }
}
