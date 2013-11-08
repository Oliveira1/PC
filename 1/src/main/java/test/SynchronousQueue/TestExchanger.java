package test.SynchronousQueue;

import static org.junit.Assert.*;

import java.util.concurrent.CountDownLatch;

import org.junit.Before;
import org.junit.Test;

import concurrency.Exchanger;

public class TestExchanger {

	@Test 
	public void withTwoThreads() throws InterruptedException{
		final Exchanger<Integer> exchanger=new Exchanger<Integer>();
		final int MAXPROC=8;
		final Integer VALUE=1;
		final int COUNT=30;

		Thread first=new Thread(){
			public void run(){
				try {
					Integer v=exchanger.exchange(2, 100000);
					assertNotNull(v);
					assertEquals(v, VALUE);
				} catch (InterruptedException e) {/*impossible*/}
			}
		};

		first.start();
		Integer v=exchanger.exchange(VALUE, 100000);
		first.join();
		assertNotNull(v);
		assertEquals(v,new Integer(2));
	}

	@Test
	public void withLotsOfThreads() throws InterruptedException{
		final Exchanger<Integer> exchanger=new Exchanger<Integer>();
		final int MAXPROC=8;
		final int[][] threadscounterMatrix=new int[MAXPROC][MAXPROC];
		final Integer VALUE=1;
		final int COUNT=30;
		final CountDownLatch notStarted=new CountDownLatch(MAXPROC);
		final CountDownLatch waitToCount=new CountDownLatch(MAXPROC);

		for (int i=0;i<MAXPROC;i++){

			Thread x=new Thread(){
				public void run(){
					Integer v;
					try {
						notStarted.countDown();
						notStarted.await();
						int i=Integer.parseInt(Thread.currentThread().getName());
						v = exchanger.exchange(i, 100000);
						assertNotNull(v);
						threadscounterMatrix[i][v]++;
					} catch (InterruptedException e) {/*Not Testing*/}
					waitToCount.countDown();
				}
			};
			x.setName(Integer.toString(i));
			x.start();
		}
		waitToCount.await();
		for(int i=0;i<(MAXPROC-1);i++)
				for(int j=i+1;j<MAXPROC;j++)
				if(threadscounterMatrix[i][j]!=threadscounterMatrix[j][i])
						throw new AssertionError();
	}
}
