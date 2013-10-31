import java.util.LinkedList;


public class SynchronousQueue<T> {

	private final LinkedList<Request<T>> _blocking =new LinkedList<Request<T>>();
	private class Request<T>{
		T item;
		boolean taken;
	}
	 // falta verificar o primeiro elemento se é null para saber se e um take
	public void put(T obj) throws InterruptedException{
		synchronized(_blocking){
			if(!_blocking.isEmpty()){
				putMessage(obj);
				_blocking.notifyAll();
				return;
			}
			Request<T> myReq=new Request<T>();
			myReq.item=obj;
			_blocking.add(myReq);

			while(true){
				_blocking.wait();
				if(myReq.taken)return;
			}
		}
	}



	private void putMessage(T obj) {
		Request<T> req=_blocking.removeFirst();
		req.item=obj;
		req.taken=true;
	}



	public T take() throws InterruptedException{
		synchronized (_blocking) {
			T item =null;
			if(!_blocking.isEmpty() && !_blocking.){
				item=takeMessage();
				_blocking.notifyAll();
				return item;
			}
			Request<T> myReq=new Request<T>();
			_blocking.add(myReq);
			while(true){
				_blocking.wait();
				if(myReq.taken){
					item=myReq.item;
					_blocking.notifyAll();
					return item;
				}
			}

		}
	}



	private T takeMessage() {
		Request<T> req=_blocking.removeFirst();
		req.taken=true;
		return req.item;
	}

}
