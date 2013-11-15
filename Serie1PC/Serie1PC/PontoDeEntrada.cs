using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie1PC
{
    class PontoDeEntrada
    {
        public static void Main()
        {
            ReentrantLock _lock=new ReentrantLock();
            Condition cond = _lock.NewCondition();

            Thread c=new Thread(() =>
            {
                _lock.Lock();
                cond.Wait();
                Console.WriteLine("AQUI ");
                _lock.Unlock();
                
            });
            
            Thread l=new Thread(() =>
            {
                Thread.Sleep(1000);
              _lock.Lock();
                cond.Pulse();
                _lock.Unlock();
            });

            c.Start();
            l.Start();
            Thread.Sleep(5000);
            Console.WriteLine("Pilihla");
        }
    }
}
