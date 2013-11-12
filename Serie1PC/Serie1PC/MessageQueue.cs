using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Serie1PC
{
    public class MessageQueue<T>
    {
        private  LinkedList<Message<T>> _senders = new LinkedList<Message<T>>();
        private  LinkedList<Message<T>> _message = new LinkedList<Message<T>>();
        private readonly LinkedList<Message<T>> _receivers = new LinkedList<Message<T>>();
        private readonly int _capacity;

        public MessageQueue(int capacity)
        {
            _capacity = capacity;
        }

        public void Send(Message<T> msg)
        {
            lock (this)
            {
                if (_receivers.Count != 0)
                {
                    if (SetMessage(msg)) return;
                }
                if (_message.Count != _capacity)
                {
                    _message.AddLast(msg);
                    return;
                }
                var myMsg = new Message<T>();
                myMsg.content = msg.content;
                myMsg.type = msg.type;
                _senders.AddLast(myMsg);
                while (true)
                {
                    Monitor.Wait(this);
                    if (myMsg.taken) return;
                    if (_message.Count != _capacity)
                    {
                        _message.AddLast(msg);
                        Monitor.PulseAll(this);
                        return;
                    }
                }
            }
        }
        
        public T Receive(Predicate<uint> predicate)
        {
            lock(this){
               var myMsg= new Message<T>();
            if (_message.Count != 0)
            {
               getMessage(predicate,ref myMsg,ref _message);
               return myMsg.content;
            }

                if (_senders.Count != 0)
                {
                    getMessage(predicate,ref myMsg,ref _senders);
                    return myMsg.content;
                }
                myMsg.pred = predicate;
                _receivers.AddLast(myMsg);
            while (true)
            {
                Monitor.Wait(this);
                if (myMsg.taken) return myMsg.content;
            }
          }
        }

        private void getMessage(Predicate<uint> predicate, ref Message<T> myMsg, ref LinkedList<Message<T>> list)
        {
            foreach (Message<T> elem in list)
            
                if (predicate.Invoke(elem.type))
                {
                    myMsg.content = elem.content;
                    elem.taken = true;
                    _receivers.Remove(elem);
                    Monitor.PulseAll(this);
                    return;
                }
            
        }
        //Metodo específico de delegação Execução de Threads
        private Boolean SetMessage(Message<T> msg)
        {
            foreach(Message<T> elem in _receivers)
                if (elem.pred.Invoke(msg.type))
                {
                    elem.content = msg.content;
                    elem.taken = true;
                    _receivers.Remove(elem);
                    Monitor.PulseAll(this);
                    return true;
                }
            return false;
        }
    }
}