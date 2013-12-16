using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NonBlockingCompletion
{
   public class NonBlockingConcurrentQueue<T>
    {

      private sealed class Node
       {
           public Node _next;
           public T _elem;
           public Node(T elem, Node next)
           {
                _elem = elem;
                _next = next;
           }
        }


	    private readonly Node _dummy = new Node(default(T), null);
       private Node _head;
       private  Node _tail;

	public NonBlockingConcurrentQueue()
	{
	    _head = _dummy;
	    _tail = _dummy;
	}

	public void put (T item){
		Node newNode = new Node(item, null); 
		while (true)
		{
		    Node curTail = _tail;
		    Node tailNext = curTail._next;
			if (curTail == _tail) { 
				if (tailNext != null) { 
					// Queue in intermediate state, advance tail 
				    Interlocked.CompareExchange(ref _tail, tailNext, curTail);
				} else { 
					// In quiescent state, try inserting new node
				    if (Interlocked.CompareExchange(ref curTail._next, newNode, null) == null)
				    {
				        // Insertion succeeded, try advancing tail 
				        Interlocked.CompareExchange(ref _tail, newNode, curTail);
				        return;
				    }
				} 
				} 
			} 
		} 


	
	public T tryTake(){

		do
		{
		    Node curHead = _head;
		    Node headNext = curHead._next;
		    Node curTail = _tail;

			if(headNext==null)return default(T);

			//estado intermedio
			if(curHead==curTail)	
		     Interlocked.CompareExchange(ref _tail, headNext, curTail);
			
			//estado normal
			else if(Interlocked.CompareExchange(ref _head,curHead,headNext)==null)
				return headNext._elem;



		}while(true);

	}

	public Boolean isEmpty()
	{
	    return (_head._next == null);
	}



    }
}
