using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Serie1PC
{
   public class Logger
    {
        private Thread _loggerThread;
        private readonly TextWriter _writeBuffer;
        private  LinkedList<String> _logsQueue = new LinkedList<String>();
        private  Status _stop;

       private  enum Status
       {
           Running=0,
           Stopping,
           Stopped
       }

       private const int CAPACITY= 10;

        public Logger(TextWriter writeTo)
        {

            if (writeTo == null)
                throw new NullReferenceException();
            _writeBuffer = writeTo;
            _stop = Status.Running;
            InitLogger();
           }

        private void InitLogger()
        {
            _loggerThread = new Thread(Write) {Name = "Logger", Priority = ThreadPriority.Lowest};
        }

        public void Start()
        {
            if(_stop==Status.Running && !_loggerThread.IsAlive)
                _loggerThread.Start();
        }

        public void Stop()
        {
            lock (this)
            {
                if (!_loggerThread.IsAlive) return;
                _stop = Status.Stopping; 
                while (true)
                {
                    Monitor.Wait(this);
                    if (_stop == Status.Stopped) return;
                }
            }
        }

        public Boolean LogMessage(String msg)
        {
            if (_stop==Status.Stopped) throw new Exception(); //undefined
            lock(this)
            {
                if (_logsQueue.Count < CAPACITY)
                {
                    _logsQueue.AddLast(msg);
                    return true;
                }
                if (_logsQueue.Count == CAPACITY)
                {
                    _logsQueue.RemoveFirst();
                    _logsQueue.AddLast(msg);
                    return true;
                }
            }
            return false;
        }

        private void Write()
        {
           var buffer=new LinkedList<string>();
            while (true)
            {
                lock (this)
                {
                    if (_logsQueue.Count != 0)
                    {
                        buffer = _logsQueue;
                        _logsQueue = new LinkedList<string>();
                    }
                }
                foreach (var elem in buffer)
                {
                    _writeBuffer.Write(elem);
                }
                lock (this)
                {
                    if (_stop == Status.Stopping && _logsQueue.Count == 0)
                    {
                        _stop = Status.Stopped;
                        Monitor.PulseAll(this);
                        return;
                    }
                }
            }
        }
    }
}
