package tests;

import static org.junit.Assert.*;

import java.util.concurrent.CountDownLatch;

import org.junit.Assert;
import org.junit.Test;

import concurrency.RendezvousChannel;
import concurrency.RendezvousChannel.Token;


public class TestRendezvousChannel {
	@Test
	public void WithOneClientAndOneServerThreadByRunningServerFirst() throws InterruptedException
	{   
		final RendezvousChannel<Integer, Integer> synchronizer = new RendezvousChannel<Integer, Integer>();
		final CountDownLatch waitingToStart = new CountDownLatch(1);
		Thread server=new Thread(){

			public void run() {
				waitingToStart.countDown();
				Token myToken;
				try {
					myToken = (Token)synchronizer.Accept(Integer.MAX_VALUE);
					synchronizer.Reply(myToken,(Integer)myToken.service*2);
				} catch (InterruptedException e) {/*impossible*/}

			}

		};
		server.start();
		Integer response;
		waitingToStart.await();
		if ((response=(Integer)synchronizer.Request(2, Integer.MAX_VALUE))!=null)
		{

			assertEquals(new Integer(4),response);
		}
	}

	@Test
	public void WithOneClientAndOneServerThreadByRunningClientFirst() throws InterruptedException
	{
		final RendezvousChannel<Integer, Integer> synchronizer = new RendezvousChannel<Integer,Integer>();
		final CountDownLatch waitingToStart = new CountDownLatch(1);
		final CountDownLatch waitingToEnd = new CountDownLatch(1);
		final  Integer result=4;
		Thread client =new Thread(){
			public void run() {
				
				waitingToStart.countDown();
				try {
					Integer response=(Integer)synchronizer.Request(2, Integer.MAX_VALUE);
					assertEquals(result, response);
					waitingToEnd.countDown();
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}

			}
		};

		client.start();
		waitingToStart.await();
		int service;
		Token myToken = (Token)synchronizer.Accept(Integer.MAX_VALUE);
		synchronizer.Reply(myToken,(Integer) myToken.service * 2);
		waitingToEnd.await();
		assertEquals(new Integer(4), result);

	}
}



