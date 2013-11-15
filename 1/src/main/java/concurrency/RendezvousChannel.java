package concurrency;

import java.util.LinkedList;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

public class RendezvousChannel<S,R> {	
	public class Token
	{
		public S service;
		public R response;

		public Token(S service)
		{
			this.service = service;
			this.response = null;
		}



		public Token()
		{
			service = null;
			response = null;
		}
	}
	private class Ticket
	{
		public Token token;
		private Status processing;
		public Condition _condition; // Lock local

		public Ticket()
		{
			token = new Token();
			processing = Status.Open;
		}
	}

	private final ReentrantLock _lock; 
	private final LinkedList<Ticket> _services;
	private final LinkedList<Ticket> _processingRequests;

	public enum Status
	{
		Open,
		Accepting,
		InProcess,
		Concluded
	};

	public RendezvousChannel()
	{
		_lock=new ReentrantLock();
		_services=new LinkedList<Ticket>();
		_processingRequests = new LinkedList<Ticket>();
	}

	public R Request(S service,int timeout) throws InterruptedException
	{
		try
		{
			_lock.lock();
			// lock
			Ticket myTicket;
			if ((myTicket=CheckForAcceptingTokens(service))!=null)
				myTicket._condition.signal();
			else
			{
				myTicket=new Ticket();
				myTicket.token.service=service;
				myTicket._condition = _lock.newCondition();
				_services.addLast(myTicket);
			}

			while (true)
			{
				myTicket._condition.await();
				if (myTicket.processing == Status.Concluded)
				{
					R response = myTicket.token.response;
					return response;
				}
			}
		}
		finally{
			_lock.unlock();
		}

	}
	private Ticket CheckForAcceptingTokens(S service)
	{
		for(Ticket ticket : _services)
		{
			if (ticket.processing == Status.Accepting)
			{
				ticket.token.service = service;
				ticket.processing=Status.InProcess;
				_services.remove(ticket);
				_processingRequests.addLast(ticket);
				return ticket;
			}
		}
		return null;
	}

	public  Object Accept(int timeout) throws InterruptedException
	{
		try{
			_lock.lock();
			
			if (!_services.isEmpty())
				return RemoveFirstRequestedToken();
			
			Ticket myTicket = new Ticket();
			myTicket._condition=_lock.newCondition();
			myTicket.processing = Status.Accepting;
			_services.addLast(myTicket);
			while (true)
			{
				myTicket._condition.await();
				if (myTicket.processing == Status.InProcess)
					return myTicket.token;

			}
		}
		finally{
			_lock.unlock();
		}
	}
	public void Reply(Object rendezVousToken, R Response)
	{
		try{
			_lock.lock();
			Token myToken = (Token) rendezVousToken;
			FindTokenInProcessingRequests(myToken, Response);       
		}
		finally{
			_lock.unlock();
		}

	}
	private Token RemoveFirstRequestedToken()
	{
		for (Ticket ticket :_services)
		{
			if (ticket.processing == Status.Open)

				ticket.processing = Status.InProcess;
			_services.remove(ticket);
			_processingRequests.addLast(ticket);
			return ticket.token;
		}
		return null;
	}

	private void FindTokenInProcessingRequests(final Token myToken, R response)
	{

		for(Ticket ticket :_processingRequests)
		{
			if (ticket.token.equals(myToken))
			{
				myToken.response = response;
				ticket.processing = Status.Concluded;
				_processingRequests.remove(ticket);
				ticket._condition.signal();           
				return;
			}
		}
	}
}

