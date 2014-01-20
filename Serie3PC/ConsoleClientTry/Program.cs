using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClientTry
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true) { 
            try
            {
                TcpClient tcp = new TcpClient("localhost", 8080);
                NetworkStream s = tcp.GetStream();
                byte[] buffer = new byte[1024];
                String l = "REGISTER\nCenas:localhost:1214";
                s.Write(Encoding.ASCII.GetBytes(l), 0, l.Length);
                Console.ReadLine();
                Console.WriteLine("CENAS");
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
}
