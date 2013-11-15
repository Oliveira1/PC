package concurrency;
import java.util.LinkedList;


public class Exchanger<T> {
	private Request<T> _request;
	private class Request<T>{
		T item;
		boolean taken;
	}
	public synchronized T exchange(T myMsg, int timeout) throws InterruptedException{
			if(_request!=null){
				T item=_request.item;
				_request.item=myMsg;
				_request.taken=true;
				_request=null;
				notifyAll();
				return item;
			}
			Request<T> myReq=new Request<T>();
			myReq.item=myMsg;
			_request=myReq;
			long lastTime=(timeout !=Integer.MAX_VALUE) ? System.currentTimeMillis(): 0;
			while(true){
				try{
				wait();
				if(myReq.taken){
					return myReq.item;
				}
				
				timeout=SyncUtils.AdjustTimeout(lastTime,timeout);
				lastTime =System.currentTimeMillis() ;
				if(timeout==0)
                {
					_request=null;
					return null;
                }
				}catch(InterruptedException e){
					if(myReq.taken)
						return myReq.item;
					
					_request=null;
					Thread.currentThread().interrupt();
					return null;
				}
			}
		}
	
}
