using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie1PC
{
    class Logger
    {
        private Thread _loggerThread;
        private readonly TextWriter _writeBuffer;
        private readonly LinkedList<String> _logsQueue = new LinkedList<String>();
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

        private void Start()
        {
            if (!_loggerThread.IsAlive)
                _loggerThread.Start();
        }

        private void Stop()
        {
            if (!_loggerThread.IsAlive) return;
            _stop = true;
        }

        private void LogMessage(String msg)
        {
            if (_stop) throw new Exception(); //undefined
            lock(_logsQueue)
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
                while (true)
                {
                    Monitor.Wait(_logsQueue);
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
                  buffer = new LinkedList<string>(_logsQueue);
                        //copia para o stack para não estar dependente do IO e minimizar o tempo de lock
                    _logsQueue.Clear();
                }
                Monitor.PulseAll(_logsQueue); //qual a dif entre por isto dentro e fora do lock?
                foreach (var elem in buffer) // este buffer por ir a null? acho que não
                {
                    _writeBuffer.Write(elem);
                }
                if (_stop && _logsQueue.Count == 0) return;
            }
        }
    }
}
