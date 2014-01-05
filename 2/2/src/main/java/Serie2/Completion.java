package Serie2;

import java.util.concurrent.atomic.AtomicInteger;

public class Completion {
	private final 	AtomicInteger permits=new AtomicInteger();

	public void waitForCompletion() throws InterruptedException{
		do{
			int curPermits=permits.get();
			if(curPermits==-1) return;
			if(curPermits>0)
				if(permits.compareAndSet(curPermits, curPermits-1))
					return;

			if(permits.get()>0) continue;
			synchronized(permits){
				permits.wait();
			}
		}while(true);

	}
	public void complete(){
		do{
			int curPermits=permits.get();
			if(curPermits==-1) return;
			if(permits.compareAndSet(curPermits, curPermits+1)){
				synchronized(permits){
					permits.notify();
				}
				return;
			}
		}while(true);


	}
	public void completeAll(){
		int i=0;
		while(i!=-1){
			permits.set(-1);
			i=permits.get();
		}
		synchronized(permits){
			permits.notifyAll();
		}
	}
}
