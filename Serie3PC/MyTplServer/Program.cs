/*
 * INSTITUTO SUPERIOR DE ENGENHARIA DE LISBOA
 * Licenciatura em Engenharia Informática e de Computadores
 *
 * Programação Concorrente - Inverno de 2009-2010
 * Paulo Pereira
 *
 * Código base para a 3ª Série de Exercícios.
 *
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MyTplServer
{
    /// <summary>
    /// Handles client requests.
    /// </summary>
    public sealed class Handler
    {
        #region Message handlers

        /// <summary>
        /// Data structure that supports message processing dispatch.
        /// </summary>
         private static readonly Dictionary<string, Action<Stream,StreamReader, StreamWriter, Logger>> MESSAGE_HANDLERS;
       // private static readonly Dictionary<string, Action<Stream, Logger>> MESSAGE_HANDLERS;


        static Handler()
        {
            MESSAGE_HANDLERS = new Dictionary<string, Action<Stream,StreamReader, StreamWriter, Logger>>();
          //  MESSAGE_HANDLERS = new Dictionary<string, Action<Stream, Logger>>();
            MESSAGE_HANDLERS["REGISTER"] = ProcessRegisterMessage;
            MESSAGE_HANDLERS["UNREGISTER"] = ProcessUnregisterMessage;
            MESSAGE_HANDLERS["LIST_FILES"] = ProcessListFilesMessage;
            MESSAGE_HANDLERS["LIST_LOCATIONS"] = ProcessListLocationsMessage;
        }

        /// <summary>
        /// Handles REGISTER messages.
        /// </summary>
        private static async void ProcessRegisterMessage(Stream con,StreamReader input, StreamWriter output, Logger log)
        {
            // Read message payload, terminated by an empty line. 
            // Each payload line has the following format
            // <filename>:<ipAddress>:<portNumber>
            string line = await input.ReadLineAsync();
            Task.Factory.StartNew(async () =>
            {
                while (!string.IsNullOrEmpty(line))
                {
                    if (line == String.Empty) return;
                    string[] triple = line.Split(':');
                    if (triple.Length != 3)
                    {
                        log.LogMessage("Handler - Invalid REGISTER message.");
                        return;
                    }
                    IPAddress ipAddress = IPAddress.Parse(triple[1]);
                    ushort port;
                    if (!ushort.TryParse(triple[2], out port))
                    {
                        log.LogMessage("Handler - Invalid REGISTER message.");
                        return;
                    }
                    //  if(triple[0]!=string.Empty && triple[1]!=string.Empty && triple[2]!=string.Empty)
                    Store.Instance.Register(triple[0], new IPEndPoint(ipAddress, port));
                    line = await input.ReadLineAsync();
                }
            });
            // This request message does not have a corresponding response message, hence, 
            // nothing is sent to the client.
        }

        /// <summary>
        /// Handles UNREGISTER messages.
        /// </summary>
        private static async void ProcessUnregisterMessage(Stream con,StreamReader input, StreamWriter output, Logger log)
        {
            // Read message payload, terminated by an empty line. 
            // Each payload line has the following format
            // <filename>:<ipAddress>:<portNumber>
            string line = await input.ReadLineAsync();
            Task.Factory.StartNew(async () =>
            {
                while (!string.IsNullOrEmpty(line))
                {

                    string[] triple = line.Split(':');
                    if (triple.Length != 3)
                    {
                        log.LogMessage("Handler - Invalid UNREGISTER message.");
                        return;
                    }
                    IPAddress ipAddress = IPAddress.Parse(triple[1]);
                    ushort port;
                    if (!ushort.TryParse(triple[2], out port))
                    {
                        log.LogMessage("Handler - Invalid UNREGISTER message.");
                        return;
                    }
                    Store.Instance.Unregister(triple[0], new IPEndPoint(ipAddress, port));
                    line = await input.ReadToEndAsync();
                }
            });

            // This request message does not have a corresponding response message, hence, 
            // nothing is sent to the client.
        }

        /// <summary>
        /// Handles LIST_FILES messages.
        /// </summary>
        private static async void ProcessListFilesMessage(Stream con,StreamReader input,StreamWriter output, Logger log)
        {
            // Request message does not have a payload.
            // Read end message mark (empty line)
            input.ReadLine();

            string[] trackedFiles = Store.Instance.GetTrackedFiles();

            // Send response message. 
            // The message is composed of multiple lines and is terminated by an empty one.
            // Each line contains a name of a tracked file.
           await  Task.Factory.StartNew(()=>
            {
                foreach (string file in trackedFiles)
                    output.WriteLine(file);

                // End response and flush it.
                output.WriteLine();
                output.Flush();
            })
            ;
        }

        /// <summary>
        /// Handles LIST_LOCATIONS messages.
        /// </summary>
        private static async void ProcessListLocationsMessage(Stream con,StreamReader input, StreamWriter output, Logger log)
        {
            // Request message payload is composed of a single line containing the file name.
            // The end of the message's payload is marked with an empty line
            string line = input.ReadLine();
            input.ReadLine();
            IPEndPoint[] fileLocations = Store.Instance.GetFileLocations(line);

            // Send response message. 
            // The message is composed of multiple lines and is terminated by an empty one.
            // Each line has the following format
            // <ipAddress>:<portNumber>
        await     Task.Factory.StartNew(() =>
            {
                foreach (IPEndPoint endpoint in fileLocations)
                    output.WriteLine(string.Format("{0}:{1}", endpoint.Address, endpoint.Port));

                // End response and flush it.
                output.WriteLine();
                output.Flush();
            });
        }

        #endregion

        private readonly Stream con;
        /// <summary>
        /// The handler's input (from the TCP connection)
        /// </summary>
        private readonly StreamReader input;

        /// <summary>
        /// The handler's output (to the TCP connection)
        /// </summary>
        private readonly StreamWriter output;

        /// <summary>
        /// The Logger instance to be used.
        /// </summary>
        private readonly Logger log;

        private byte[] buffer;
        private TcpClient client;
        /// <summary>
        ///	Initiates an instance with the given parameters.
        /// </summary>
        /// <param name="connection">The TCP connection to be used.</param>
        /// <param name="log">the Logger instance to be used.</param>
        public Handler(TcpClient client, Logger log)
        {
            this.log = log;
           output = new StreamWriter(client.GetStream());
           input = new StreamReader(client.GetStream());
           buffer=new byte[4096];
           con = client.GetStream();
            this.client = client;

        }

        /// <summary>
        /// Performs request servicing.
        /// </summary>
        public async void Run()
        {
            try
            {
                string requestType;
                // Read request type (the request's first line)

               await Task.Factory.StartNew(async() => { requestType = await input.ReadLineAsync();
                while (!string.IsNullOrEmpty(requestType))
                {

                    requestType = requestType.ToUpper();
                    if (!MESSAGE_HANDLERS.ContainsKey(requestType))
                    {
                        log.LogMessage("Handler - Unknown message type. Servicing ending.");
                        return;
                    }
                    // Dispatch request processing
                    MESSAGE_HANDLERS[requestType](con, input, output, log);

                    requestType = await input.ReadLineAsync();
                    // }
                }
                log.LogMessage(" ::: " + requestType);
            })
            ;
        }
            catch (IOException ioe)
            {
                // Connection closed by the client. Log it!
                log.LogMessage(String.Format("Handler - Connection closed by client {0}", ioe));
            }
            /*finally
            {
                 input.Close();
                output.Close();
                con.Close();
                client.Close();
            }*/
        }
    }

    /// <summary>
    /// This class instances are file tracking servers. They are responsible for accepting 
    /// and managing established TCP connections.
    /// </summary>
    public sealed class Listener
    {
        /// <summary>
        /// TCP port number in use.
        /// </summary>
        private readonly int portNumber;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private readonly TcpListener srv;
        private Logger _log;
        /// <summary> Initiates a tracking server instance.</summary>
        /// <param name="_portNumber"> The TCP port number to be used.</param>
        public Listener(int _portNumber)
        {
            portNumber = _portNumber;
            srv = new TcpListener(IPAddress.Loopback, portNumber);
        }



        /// <summary>
        ///	Server's main loop implementation.
        /// </summary>
        /// <param name="log"> The Logger instance to be used.</param>
        public void Run(Logger log)
        {
            _log = log;
            try
            {
                log.LogMessage("Listener - Waiting for connection requests.");
                srv.Start();
                    srv.BeginAcceptTcpClient(Accept, null);
                    Program.ShowInfo(Store.Instance); 
            }
                catch(Exception e)
                {
                    log.LogMessage("Listener - Ending.");
                    srv.Stop();
                }
        }

        private void Accept(IAsyncResult ar)
        {
            TcpClient socket = srv.EndAcceptTcpClient(ar);
            
                socket.LingerState = new LingerOption(true, 10);
                Console.WriteLine(String.Format("Listener - Connection established with {0}.",
                    socket.Client.RemoteEndPoint));
                Handler protocolHandler = new Handler(socket, _log);
                protocolHandler.Run();
            
                srv.BeginAcceptTcpClient(Accept, null);
        }

  
    }


    internal class Program
    {
        public static void ShowInfo(Store store)
        {
            foreach (string fileName in store.GetTrackedFiles())
            {
                Console.WriteLine(fileName);
                foreach (IPEndPoint endPoint in store.GetFileLocations(fileName))
                {
                    Console.Write(endPoint + " ; ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

/*
        static void TestStore()
        {
            Store store = Store.Instance;

            store.Register("xpto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
            store.Register("xpto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
            store.Register("xpto", new IPEndPoint(IPAddress.Parse("195.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();
            store.Register("ypto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
            store.Register("ypto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();
            store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("195.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();

            store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("193.1.2.3"), 1111));
            store.Unregister("xpto", new IPEndPoint(IPAddress.Parse("194.1.2.3"), 1111));
            ShowInfo(store);
            Console.ReadLine();
        }
*/


        /// <summary>
        ///	Application's starting point. Starts a tracking server that listens at the TCP port 
        ///	specified as a command line argument.
        /// </summary>
        public static void Main(string[] args)
        {
            // Checking command line arguments
         /*   if (args.Length != 1)
            {
                Console.WriteLine("Utilização: {0} <numeroPortoTCP>", AppDomain.CurrentDomain.FriendlyName);
                Environment.Exit(1);
            }

            ushort port;
            if (!ushort.TryParse(args[0], out port))
            {
                Console.WriteLine("Usage: {0} <TCPPortNumber>", AppDomain.CurrentDomain.FriendlyName);
                return;
            }*/
            ushort port;

            port = 8080;
            // Start servicing
            Logger log = new Logger();
            log.Start();
            try
            {
                new Listener(port).Run(log);
            }
            catch(Exception e)
            {
                log.Stop();
            }
            Console.Read();
        }
    }
}
