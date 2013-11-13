using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie1PC
{
   public class Logger
    {
        private Thread _loggerThread;
        private readonly TextWriter _writeBuffer;
        private  LinkedList<String> _logsQueue = new LinkedList<String>();
        private volatile Boolean _stop;
        private const int CAPACITY= 10;

        public Logger(TextWriter writeTo)
        {

            if (writeTo == null)
                throw new NullReferenceException();
            _writeBuffer = writeTo;
            _stop = false;
            initLogger();
           }

        private void initLogger()
        {
            _loggerThread = new Thread(new ThreadStart(write));
            _loggerThread.Name = "Logger"; // for debugging purpose only
            _loggerThread.Priority = ThreadPriority.Lowest; 
        }

        public void Start()
        {
            if (!_loggerThread.IsAlive)
                _loggerThread.Start();
        }

        public void Stop()
        {
            if (!_loggerThread.IsAlive) return;
            _stop = true;
        }

        public void LogMessage(String msg)
        {
            if (_stop) throw new Exception(); //undefined
            lock(this)
            {
                if (_logsQueue.Count < CAPACITY)
                {
                    _logsQueue.AddLast(msg);
                    return;
                }
                if (_logsQueue.Count == CAPACITY && !_loggerThread.IsAlive)
                {
                    _logsQueue.RemoveFirst();
                    _logsQueue.AddLast(msg);
                    return;
                }
                if (_logsQueue.Count == CAPACITY && _loggerThread.IsAlive)
                {
                    _logsQueue.RemoveFirst();
                    _logsQueue.AddLast(msg);
                    return;
                }
                while (true)
                {
                    Monitor.Wait(this);
                    if (_stop) throw new Exception(); //undefined
                    if (_logsQueue.Count < CAPACITY)
                    {
                        _logsQueue.AddLast(msg);
                        return;
                    }
                }
            }
        }

        private void write()
        {
           LinkedList<string> buffer;
            while (true)
            {
                lock (_logsQueue)
                {
                    buffer = _logsQueue;
                    _logsQueue=new LinkedList<string>();
                  //buffer = new LinkedList<string>(_logsQueue);
                        //copia para o stack para não estar dependente do IO e minimizar o tempo de lock
                  
                }
                Monitor.Pulse(this);
                foreach (var elem in buffer) // este buffer por ir a null? acho que não
                {
                    _writeBuffer.Write(elem);
                }
                if (_stop && _logsQueue.Count == 0) Monitor.Wait(this);
            }
        }
    }
}
