package concurrency;
import java.util.LinkedList;


public class Exchanger<T> {
	private Request<T> _request;
	private class Request<T>{
		T item;
		boolean taken;
	}
	public synchronized T exchange(T myMsg, long timeout) throws InterruptedException{
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
			while(true){
				wait();
				if(myReq.taken){
					return myReq.item;
				}
			}
		}
	
}
