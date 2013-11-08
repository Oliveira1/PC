package concurrency;
import java.util.LinkedList;


public class Exchanger<T> {
	private Request<T> _request;
	private class Request<T>{
		T item;
		boolean taken;
	}

	public T exchange(T myMsg, long timeout) throws InterruptedException{
		synchronized (_request) {
			if(_request!=null){
				T item=_request.item;
				_request.item=myMsg;
				_request.taken=true;
				notifyAll();
				return item;
			}
			Request<T> myReq=new Request<T>();
			myReq.item=myMsg;
			_request=myReq;

			while(true){
				_request.wait();

				if(myReq.taken){
					_request=null;
					return myReq.item;
				}
			}
		}
	}
}
