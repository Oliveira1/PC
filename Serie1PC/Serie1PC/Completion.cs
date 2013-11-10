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

        public void WaitForCompletion()
        {
            lock (this)
            {
                if (_complete) return;
                if (_permits > 0)
                {
                    _permits--;
                    return;
                }

                while (true)
                {
                    Monitor.Wait(this);
                    if (_complete) return;
                    if (_permits > 0)
                    {
                        _permits--;
                        return;
                    }

                }

            }
        }

    }
}
