package SynchronousQueueTests;

import static org.junit.Assert.*;

import org.junit.Test;

import concurrency.SynchronousQueue;
public class TestWithTwoThreads {

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
	public void withOnePutterandSeverallTakers(){
		final SynchronousQueue<Integer> sync=new SynchronousQueue<Integer>();
		 final Integer VALUE=1;
		 
		
	}
	
}
