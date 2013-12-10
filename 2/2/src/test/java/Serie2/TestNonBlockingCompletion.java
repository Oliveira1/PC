package Serie2;

import static junit.framework.Assert.assertEquals;
import static junit.framework.Assert.assertTrue;

import java.util.concurrent.CountDownLatch;

import org.junit.Test;

public class TestNonBlockingCompletion {

	
	
	

	@Test
	public void OneWaitOneComplete() throws InterruptedException{
	final Completion sync=new Completion();
	
	Thread x=new Thread(){
		public void run(){
		
			try {
				sync.waitForCompletion();
			} catch (InterruptedException e) {	/* IMPOSSIBLE*/	}
		}
		
	};
	
	x.start();
	
	sync.complete();
	x.join();
	
	}
	
	@Test
	public void SeverallWaitsAndC() throws InterruptedException{
		final Completion sync=new Completion();
		final int MAXPROCS=8;
		final CountDownLatch waitToInvokeComplete=new CountDownLatch(MAXPROCS);
		final CountDownLatch waitingToEnd=new CountDownLatch(MAXPROCS);
		
		for(int i=0;i<MAXPROCS;i++){
			new Thread(){
				public void run(){
					try {
						waitToInvokeComplete.countDown();
						sync.waitForCompletion();
						waitingToEnd.countDown();
					} catch (InterruptedException e) {	/* IMPOSSIBLE*/	}
				}
			}.start();
			
			
		}
		
		waitToInvokeComplete.await();
	
		for(int i=0;i<MAXPROCS;i++)
			sync.complete();
		waitingToEnd.await();
		
	}
}
