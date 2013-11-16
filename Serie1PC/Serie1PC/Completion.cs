using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie1PC
{
    public class Completion
    {
        private int _permits;
        private Boolean _complete;
        /*
         * Sinaliza a conclusão de uma Tarefa
         * Viabiliza a chamada de um WaitForCompletion
         */

        public Completion()
        {
            _permits = 0;
            _complete = false;
        }
        public void Complete()
        {
            lock (this)
            {
                if (_complete) return;
                _permits++;
                Monitor.Pulse(this);
            }
        }
        /*
         * viabiliza todas as chamadas anteriores ou posteriores a WaitForCompletion
         * 
         */
        public void CompleteAll()
        {
            lock (this)
            {
                if (_complete) return;
                _complete = true;
                Monitor.PulseAll(this);
            }
        }

        public bool WaitForCompletion(int timeout)
        {
            lock (this)
            {
                if (_complete) return true;
                if (_permits > 0)
                {
                    _permits--;
                    return true ;
                }
                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
                while (true)
                {
                    try
                    {
                        Monitor.Wait(this);
                        if (_complete) return true;
                        if (_permits > 0)
                        {
                            _permits--;
                            return true;
                        }
                        if (SyncUtils.AdjustTimeout(ref lastTime, ref timeout) == 0)
                        {
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        Thread.CurrentThread.Interrupt();
                        throw new ThreadInterruptedException();
                    }
                }

            }
        }

    }
}
