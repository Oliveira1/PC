using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CreatingConditionLikeJava
{
    public sealed class Condition
    {
        private readonly object _cond;
        private readonly ReentrantLock _master;

        public Condition(ReentrantLock master)
        {
            _master = master;
        }

        /* sei que o master is locked by me*/
        public void Pulse()
        {
            ThreadInterruptedException tie = enterMon(_cond);
            Monitor.Enter(_cond); //Não pode falhar porque não pode estar a lancar excepcoes de cancelamento
            Monitor.Pulse(_cond);
            Monitor.Exit(_cond);
        }

        public void Wait()
        {
            Monitor.Enter(_cond);
            _master.Unlock();
            try
            {
                Monitor.Wait(_cond);
            }
            finally
            {
                _master.UninterruptibleLock();
                Monitor.Exit(_cond);
            }
        }

        private ThreadInterruptedException enterMon(Object mon)
        {
            ThreadInterruptedException tie = null;
            while (true)
            {
                try
                {
                    Monitor.Enter(mon);
                    return tie;
                }
                catch(ThreadInterruptedException e)
                {
                    tie = e;
                }
            }

        }

    }
}
