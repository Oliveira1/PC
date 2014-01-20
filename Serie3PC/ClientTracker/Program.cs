using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientTracker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
      //  [STAThread]
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            try
            {
                TcpClient tcp = new TcpClient("localhost", 8080);
                NetworkStream s = tcp.GetStream();
                byte[] buffer = new byte[1024];
                String l = "Received\nCenas:localhost: ";
                s.Write(Encoding.ASCII.GetBytes(l), 0, l.Length);
                Console.ReadLine();
                l = "Cenas:localhost:";
                s.Write(Encoding.ASCII.GetBytes(l), 0, l.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("exception");
            }
        }
    }
}
