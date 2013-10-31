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
        public class Message<T>
        {
            public  Predicate<uint> pred;
            public  uint value;
            public  T msg;
            public Boolean taken;
        }
        private  LinkedList<Message<T>> _senders = new LinkedList<Message<T>>();
        private  LinkedList<Message<T>> _message = new LinkedList<Message<T>>();
        private  LinkedList<Message<T>> _receivers = new LinkedList<Message<T>>();
        private  int _capacity;


        /*
         * Duvida sobre este Sincronizador se há delegação de execução as threads esperam numa fila _blocked 
         * se não é preciso delegação de execução podem esperar na fila de _message;
         */
        public MessageQueue(int capacity)
        {
            _capacity = capacity;
        }

        public void send(Message<T> msg)
        {
            lock (this)
            {
                if (_message.Count < _capacity)
                {
                    if (_receivers.Count == 0)
                    {
                        _message.AddLast(msg);
                        return;
                    }
                    if (SetMessage(msg)) return;
                }
                Message<T> myMsg = new Message<T>();
                myMsg.msg = msg.msg;
                myMsg.value = msg.value;
                _senders.AddLast(myMsg);
                while (true)
                {
                    Monitor.Wait(this);
                    if (myMsg.taken) return;
                    if (_message.Count < _capacity)
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
                Message<T> myMsg= new Message<T>();
            if (_message.Count != 0)
            {
               getMessage(predicate,ref myMsg,ref _message);
               Monitor.PulseAll(this);
               return myMsg.msg;
            }

                if (_senders.Count != 0)
                {
                    getMessage(predicate,ref myMsg,ref _senders);
                    Monitor.PulseAll(this);
                    return myMsg.msg;
                }
                myMsg.pred = predicate;
                _receivers.AddLast(myMsg);
            while (true)
            {
                Monitor.Wait(this);
                if (myMsg.taken) return myMsg.msg;
            }
          }
        }

        private void getMessage(Predicate<uint> predicate, ref Message<T> myMsg, ref LinkedList<Message<T>> list)
        {
            foreach (Message<T> elem in list)
            
                if (predicate(elem.value))
                {
                    myMsg.msg = elem.msg;
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
                if (elem.pred(msg.value))
                {
                    elem.msg = msg.msg;
                    _receivers.Remove(elem);
                    Monitor.PulseAll(this);
                    return true;
                }
            return false;
        }
    }
}