using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TestClient;

namespace PROFTESTCLIENT
{
    public partial class clientview : Form
    {
        private Client cs;
        public clientview()
        {
            InitializeComponent();
             cs=new Client();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!cs.connected) return;
            try
            {
                foreach (DataGridViewRow row in datagridReg.Rows)
                {
                    try
                    {
                        String name = row.Cells[0].Value.ToString();
                        String address = row.Cells[1].Value.ToString();
                        ushort port = Convert.ToUInt16(row.Cells[2].Value);
                        cs.Register(new[] {name}, address, port);
                    }
                    catch (NullReferenceException exc)
                    {
                        //ABAFATOR
                    }
                }
                datagridReg.Rows.Clear();
            }
            catch (SocketException se)
            {
                cs.Disconnect();
                lb_connect.Text = "Disconnected";
            }
            lb_status_text.Text = "Register Completed";
        }

        private void button_ListFiles_Click(object sender, EventArgs e)
        {
            if (!cs.connected) return;
            try
            {
                FilesList.Rows.Clear();
                StringBuilder sb = new StringBuilder();
                TextWriter to = new StringWriter(sb);
                cs.ListFiles(to);
                String[] lines = sb.ToString().Split('\n');
                foreach (var line in lines)
                {
                    FilesList.Rows.Add(line);
                }
            }
            catch (SocketException se)
            {
                cs.Disconnect();
                lb_connect.Text = "Disconnected";
            }

            lb_status_text.Text = "List Files Completed";
        }

        private void button_ListLocations_Click(object sender, EventArgs e)
        {
            if (!cs.connected) return;
            try
            {
                DataGridViewRow row;
                try
                {
                    row = FilesList.SelectedRows[0];
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    lb_status_text.Text = "A Row Must Be Selected";
                    return;
                }
                FilesLocationGrid.Rows.Clear();
                StringBuilder sb = new StringBuilder();
                TextWriter to = new StringWriter(sb);
                string s = row.Cells[0].Value.ToString().Trim('\n', '\0');
                StringBuilder sss =new StringBuilder();
                char[] chars = s.ToCharArray();
                for (int i = 0; i < chars.Length; i++)
                {
                    if (Char.IsLetter(chars[i]))
                        sss.Append(chars[i]);
                }

                cs.ListLocations(sss.ToString(), to);
                to.Flush();
                String[] lines = sb.ToString().Split('\n');
                foreach (var line in lines)
                {
                    string[] columns = line.Split(':');
                    FilesLocationGrid.Rows.Add(columns);

                }
            }
            catch (SocketException se)
            {
                cs.Disconnect();
                lb_connect.Text = "Disconnected";
            }

            lb_status_text.Text = "List Locations Completed";
        }

        private void button_unregister_Click(object sender, EventArgs e)
        {
            if (!cs.connected) return;
            try
            {
                foreach (DataGridViewRow row in datagridReg.Rows)
                {
                    try
                    {
                        String name = row.Cells[0].Value.ToString();
                        String address = row.Cells[1].Value.ToString();
                        ushort port = Convert.ToUInt16(row.Cells[2].Value);
                        cs.Unregister(name, address, port);
                    }
                    catch (NullReferenceException exc)
                    {
//ABAFATOR
                    }
                }
            }
            catch (SocketException se)
            {
                cs.Disconnect();
                lb_connect.Text = "Disconnected";
            }
            lb_status_text.Text = "Unregister Completed";
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            if (cs.connected)
            {
                cs.Disconnect();
                lb_connect.Text = "Disconnected";
            }
            else
            {
                cs.Connect();
                lb_connect.Text = "Connected";
            }
        }
    }
}
