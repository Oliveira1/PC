package Serie2;

import java.util.LinkedList;
import java.util.concurrent.atomic.AtomicReference;

public class ConcurrentQueue<T> {


	private static class Node <E> { 
		final E item; 
		final AtomicReference<Node<E>> next; 
		public Node(E item, Node<E> next) { 
			this.item = item; 
			this.next = new AtomicReference<Node<E>>(next); 
		} 
	} 


	private final Node<T> dummy = new Node<T>(null, null); 
	private final AtomicReference<Node<T>> head	= new AtomicReference<Node<T>>(dummy); 
	private final AtomicReference<Node<T>> tail	= new AtomicReference<Node<T>>(dummy);

	public ConcurrentQueue(){
	}

	public void put (T item){
		Node<T> newNode = new Node<T>(item, null); 
		while (true) { 
			Node<T> curTail = tail.get(); 
			Node<T> tailNext = curTail.next.get(); 
			if (curTail == tail.get()) { 
				if (tailNext != null) { 
					// Queue in intermediate state, advance tail 
					tail.compareAndSet(curTail, tailNext); 
				} else { 
					// In quiescent state, try inserting new node 
					if (curTail.next.compareAndSet(null, newNode)) { 
						// Insertion succeeded, try advancing tail 
						tail.compareAndSet(curTail, newNode); 
						return;
					} 
				} 
			} 
		} 


	}
	public T tryTake(){

		do{
			Node<T> curHead = head.get(); 
			Node<T> headNext = curHead.next.get();
			Node<T> curTail =tail.get();

			if(headNext==null)return null;

			//estado intermedio
			if(curHead==curTail)	
				tail.compareAndSet(curTail, headNext);
			
			//estado normal
			else	if(head.compareAndSet(curHead,headNext))
				return headNext.item;



		}while(true);

	}

	public boolean isEmpty() {
		return (head.get().next.get()==null);
	}


}
