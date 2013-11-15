using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Serie1PC
{
    class SemaforoFifo
    {
        private readonly ReentrantLock _lock;
        private int _units;
        private readonly LinkedList<Request> _requeue;

        private class Request
        {
            public int _missingUnits;
            public Condition _condition;
        }

        public SemaforoFifo(int units)
        {
            _units = units;
            _requeue = new LinkedList<Request>();
            _lock=new ReentrantLock();
        }

        public void P(int units)
        {
            try
            {
                _lock.Lock();
                if (_units >= units)
                {
                    _units -= units;
                    return;
                }
                units -= _units;
                _units = 0;
                Request myRequest=new Request();
                myRequest._missingUnits = units;
                myRequest._condition=new Condition(_lock);
                _requeue.AddLast(myRequest);
                while (true)
                {
                    myRequest._condition.Wait();
                    if (myRequest._missingUnits == 0) return;
                }
            }
            finally
            {
                _lock.Unlock();
            }
        }

        public void V(int units)
        {
            try
            {
                _lock.Lock();



                while (_requeue.Count != 0 && units > 0)
                {
                    Request first = _requeue.First();
                    if (units >= first._missingUnits)
                    {
                        units -= first._missingUnits;
                        first._missingUnits = 0;
                        _requeue.RemoveFirst();
                        first._condition.Pulse();
                    }
                    else
                    {
                        _requeue.First()._missingUnits -= units;
                        units = 0;
                        return;
                    }
                }
                _units += units;
            }
            finally
            {
                _lock.Unlock();
            }
        }
    }

}
