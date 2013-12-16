using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NonBlockingCompletion
{
    class NonBlockingCompletion
    {
        static void Main(string[] args)
        {
        }



        private  volatile int permits = 0;

        public void WaitForCompletion()
        {
            
            do
            {
                var curPermits = permits;
			if(curPermits==-1) return;
			if(curPermits>0)
				if(Interlocked.CompareExchange(ref permits,curPermits,permits-1)==curPermits)
                    if(permits==(curPermits-1))
					    return;

			if(permits>0) continue;
                lock (this)
                {

                    Monitor.Wait(this);

                }

		}while(true);
        }


        public void complete(){
		do{
			int curPermits=permits;
			if(curPermits==-1) return;
		    if (Interlocked.CompareExchange(ref permits, curPermits, permits - 1) == curPermits)
		    {
		        if (permits == (curPermits + 1))
		            lock (this)
		            {
		                Monitor.Pulse(this); 
                        return;
		            }
		    }
			
		}while(true);


	}
        public void completeAll(){
		int i=0;
		while(permits!=-1){
			permits=-1;
		}
		lock(this){
			Monitor.PulseAll(this);
		}
	}


    }
}
