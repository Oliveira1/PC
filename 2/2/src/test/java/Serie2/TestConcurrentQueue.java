package Serie2;

import static junit.framework.Assert.*;
import static org.junit.Assert.assertNotNull;

import java.util.Arrays;
import java.util.concurrent.CountDownLatch;

import org.junit.Test;

import Serie2.ConcurrentQueue;

public class TestConcurrentQueue {


	@Test
	public void OnePutOneTake() throws InterruptedException{
		final ConcurrentQueue<Integer> sync=new ConcurrentQueue<Integer>();


		Thread producer=new Thread(){
			public void run(){
				sync.put(2);
			}
		};
		producer.start();

		producer.join();

		Integer value=sync.tryTake();

		assertEquals(2, value.intValue());
		assertTrue(sync.isEmpty());
	}


	@Test
	public void OnePutTwoTakes() throws InterruptedException{
		final ConcurrentQueue<Integer> sync=new ConcurrentQueue<Integer>();


		Thread producer=new Thread(){
			public void run(){
				sync.put(2);
			}
		};
		producer.start();

		producer.join();

		Integer value=sync.tryTake();
		assertEquals(2, value.intValue());
		assertTrue(sync.isEmpty());

		value=sync.tryTake();

		assertNull(value);
	}

	@Test
	public void withSeverallThreadsToTestRepetitionAvoidance() throws InterruptedException{
		final ConcurrentQueue<Integer> sync=new ConcurrentQueue<Integer>();
		final int MAXPROC=8;
		final int[] valuesOfTakes=new int[MAXPROC/2];
		final int COUNT=30;
		final CountDownLatch notStarted=new CountDownLatch(MAXPROC);
		final CountDownLatch waitToCount=new CountDownLatch(MAXPROC);

		for (int i=0;i<MAXPROC/2;i++){

			Thread x=new Thread(){
				public void run(){
					Integer v;
					try {
						notStarted.countDown();
						notStarted.await();
						
							sync.put(Integer.parseInt(Thread.currentThread().getName()));
						
					} catch (InterruptedException e) {/*Not Testing*/}
					waitToCount.countDown();
				}
			};
			x.setName(Integer.toString(i+10));
			x.start();
		}
		for(int i=0;i<MAXPROC/2;i++){
			Thread x=new Thread(){
				public void run(){
					Integer v;
					try {
						notStarted.countDown();
						notStarted.await();
						int i=Integer.parseInt(Thread.currentThread().getName());
							v=sync.tryTake();
							if(v==null)
								v=null;
							else{
								valuesOfTakes[i]=v;
							}
					} catch (InterruptedException e) {/*Not Testing*/}
					waitToCount.countDown();
				}
			};
			x.setName(Integer.toString(i));
			x.start();


		}
		waitToCount.await();
		Arrays.sort(valuesOfTakes);
		for(int i=0;i<(MAXPROC/2)-1;i++)
			assertFalse(valuesOfTakes[i]==valuesOfTakes[i+1]);
		
		assertEquals(!(valuesOfTakes[0]==0),sync.isEmpty() );	
	}
			
}
