using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using PROFTESTCLIENT;

namespace TestClient
{

    public class Client
    {
        private const int PORT = 8080;
        private TcpClient client = new TcpClient();
        public bool connected = false;
        private StreamReader input;
        private StreamWriter output;

        public void Connect()
        {
            if (client.Connected) return;
                client = new TcpClient();
                client.Connect(IPAddress.Loopback, PORT);
                output=  new StreamWriter(client.GetStream());
                input=new StreamReader(client.GetStream());
                connected = true;
            

        }

        public void Disconnect()
        {
            if (!client.Connected && !connected) return;
            connected = false;
            output.WriteLine("");
            output.WriteLine("");
            output.Close();
            input.Close();
            client.Close();

        }
        

        public  void Register(IEnumerable<string> files, string adress, ushort port) 
        {
            try{
                // Send request type line
                output.WriteLine("REGISTER");

                // Send message payload
                foreach(string file in files)
                    output.WriteLine(string.Format("{0}:{1}:{2}", file, adress, port));

                // Send message end mark
                output.WriteLine();
                output.Flush();
            }
            catch (IOException ie)
            {
                throw new SocketException();
            }
        }

        public  void Unregister(string file, string adress, ushort port)
        {
            try
            {
                // Send request type line
                output.WriteLine("UNREGISTER");
                // Send message payload
                output.WriteLine(string.Format("{0}:{1}:{2}", file, adress, port));
                // Send message end mark
                output.WriteLine();
                output.Flush();
            }
            catch (IOException ie)
            {
                throw new SocketException();
            }
            
        }

        public  void ListFiles(TextWriter writeTo)
        {
            try
            {
                // Send request type line
                output.WriteLine("LIST_FILES");
                // Send message end mark and flush it
                output.WriteLine();
                output.Flush();
                // Read response
                string line;
                while ((line = input.ReadLine()) != null && line != string.Empty)
                    writeTo.WriteLine(line);
            }
            catch (IOException ie)
            {
                throw new SocketException();
            }
        }

        public  void ListLocations(string fileName,TextWriter writeTo)
        {
            try{
                // Send request type line
                output.WriteLine("LIST_LOCATIONS");
                // Send message payload
                output.WriteLine(fileName);
                // Send message end mark and flush it
                output.WriteLine();
                output.Flush();
                // Read response
                string line;
            while ((line = input.ReadLine()) != null && line != string.Empty)
                writeTo.WriteLine(line);
            }
            catch (IOException ie)
            {
                throw new SocketException();
            }
        }

        [STAThread]
    public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new clientview());
         /*   Client cs =new Client();
            cs.Connect();
           cs.Register(new [] {"xpto", "ypto", "zpto"}, "192.1.1.1", 5555);
            //Register(new[] { "xpto", "ypto" }, "192.1.1.2", 5555);
            //Register(new[] { "xpto" }, "192.1.1.3", 5555);
            Console.ReadLine();
            Console.WriteLine("List files:");
           cs.ListFiles(Console.Out);
            Console.WriteLine("List locations xpto");
           cs.ListLocations("xpto",Console.Out);
        Console.ReadLine();
        Console.WriteLine("List locations ypto");
            cs.ListLocations("ypto",Console.Out);
        Console.ReadLine();
            cs.ListFiles(Console.Out);
        Console.ReadLine();*/
        }
    }
}
