package test.SynchronousQueue;

import static org.junit.Assert.*;

import java.util.Arrays;
import java.util.concurrent.CountDownLatch;

import junit.framework.AssertionFailedError;

import org.junit.Test;

import concurrency.SynchronousQueue;
public class TestSynchronousQueue {

	@Test
	public void withTwoThreads() throws InterruptedException{
		final SynchronousQueue<Integer> sync=new SynchronousQueue<Integer>();
		final Integer VALUE=1;
		Thread Putter =new Thread(){
			public void run(){
				try {
					sync.put(VALUE);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		};

		Putter.start();
		Integer val=sync.take();

		assertNotNull(val);
		assertEquals(VALUE, val);
		Putter.join();
	}

	@Test
	public void withSeverallPutsandSeverallTakes() throws InterruptedException{
		final SynchronousQueue<Integer> sync=new SynchronousQueue<Integer>();
		final Integer VALUE=1;
		final int COUNT=30;
		Thread Taker =new Thread(){

			public void run(){
				int count=COUNT;
				while(count--!=0){
					try {
						Integer v=sync.take();
						assertNotNull(v);
						assertEquals(VALUE, v);
						v=0;
					} catch (InterruptedException e) {/*not catching*/	}
				}

			}
		};

		Taker.start();
		int putCount=COUNT;
		while(putCount--!=0){
			sync.put(VALUE);
		}
		Taker.join();
	}

	/*DUVIDA : Como testar a fifo */
	/*BUG: actualmente pode haver um context switch entre o 
	 * registo to tempo e o take e nem sempre a thread que se 
	 * iniciou a mais tempo é a que chama o take primeiro.*/
	@Test
	public void withSeverallBlockingTakesRespectingFIFO() throws InterruptedException{
		final SynchronousQueue<Integer> sync=new SynchronousQueue<Integer>();
		final int MAXPROC=8;
		final Integer VALUE=1;
		final long[] waitingTimes=new long[MAXPROC];
		final CountDownLatch waiting=new CountDownLatch(MAXPROC);
		for(int i=0;i<MAXPROC;i++){
			Thread x=new Thread(){
				public void run(){
					waiting.countDown();
					
					try {
						long time=System.currentTimeMillis();
						Integer v=sync.take();
						assertNotNull(v);
						assertEquals(VALUE, v);
						v=0;
						waitingTimes[Integer.parseInt(Thread.currentThread().getName())]=time;
					} catch (InterruptedException e) {/*not catching*/}
						
				}
			};
			x.setName(Integer.toString(i));
			x.start();
		}
		
		waiting.await();
		int cycles=MAXPROC;
		while(cycles--!=0){
			sync.put(VALUE);
		}
			long time=waitingTimes[0];
			for(int i=1;i<MAXPROC;i++)
				if(waitingTimes[i]<time)
					throw new AssertionFailedError();
			
	}
	
	@Test 
	public void withSeverallBlockingPuts() throws InterruptedException{
		
		final SynchronousQueue<Integer> sync=new SynchronousQueue<Integer>();
		final int MAXPROC=8;
		final Integer VALUE=1;
		final long[] waitingTimes=new long[MAXPROC];
		final CountDownLatch waiting=new CountDownLatch(MAXPROC);
		for(int i=0;i<MAXPROC;i++){
			Thread x=new Thread(){
				public void run(){
					waiting.countDown();
					
					try {
						waiting.countDown();
						sync.put(VALUE);
					} catch (InterruptedException e) {/*not catching*/}
						
				}
			};
			x.setName(Integer.toString(i));
			x.start();
		}
		
		waiting.await();
		int cycles=MAXPROC;
		while(cycles--!=0){
			Integer v=sync.take();
			assertNotNull(v);
			assertEquals(VALUE, v);
			v=0;
		}
	}
}
