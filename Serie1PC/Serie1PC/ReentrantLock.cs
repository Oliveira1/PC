using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie1PC
{
    public sealed class ReentrantLock
    {
        private readonly Object _lock;

        public ReentrantLock()
        {
            _lock=new Object();
        }

        public void Lock()
        {
            Monitor.Enter(_lock);
        }

        public void Unlock()
        {
            Monitor.Exit(_lock);
        }


        public Condition NewCondition()
        {
            return new Condition(this);
        }

        public void UninterruptibleLock()
        {
            ThreadInterruptedException tie = null;
            while (true)
            {
                try
                {
                    Monitor.Enter(_lock);
                    break;
                }
                catch (ThreadInterruptedException e)
                {
                    tie = e;
                }
                if (tie != null)
                {
                    Thread.CurrentThread.Interrupt();
                }
            }
        }
    }
}
