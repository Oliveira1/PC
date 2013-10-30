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
            public sealed uint value;
            public sealed T msg;
        }

        private sealed LinkedList<Message<T>> _message = new LinkedList<Message<T>>();
        private sealed LinkedList<Message<T>> _blocked = new LinkedList<Message<T>>();
        private sealed int _capacity;


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
            lock (_message)
            {
                if (_message.Count < _capacity)
                {
                    if (_blocked.Count == 0)
                    {
                        _message.AddLast(msg);
                        return;
                    }
                   // SetMessage(msg); // se este método existir o predicate do receive deixa de fazer sentido.
                    // se não existir optimizar este troço.
                    _message.AddLast(msg);
                    Monitor.PulseAll(_message);
                    return;


                }
                while (true)
                {
                    Monitor.Wait(_message);
                    if (_message.Count < _capacity)
                    {
                        _message.AddLast(msg);
                        Monitor.PulseAll(_message);
                        return;
                    }
                }
            }
        }
        
        public Message<T> Receive(Predicate<int> predicate)
        {
            lock(_message){
                Message<T> myMsg=new Message<T>();
            if (_message.Count != 0)
            {
               myMsg=getMessage(predicate);
               Monitor.PulseAll(_message);
               return myMsg;
            }
                _blocked.AddLast(myMsg);
            while (true)
            {
                Monitor.Wait(_message);
                /*implementar depois da duvida*/
            }
          }
        }

        private Message<T> getMessage(Predicate<int> predicate)
        {
            throw new NotImplementedException();
        }
        //Metodo específico de delegação Execução de Threads
        private Boolean SetMessage(Message<T> msg)
        {
            foreach(Message<T> elem in _blocked)
                if (elem.value == msg.value)
                {
                    elem.msg = msg.msg;
                    _blocked.Remove(elem);
                    Monitor.PulseAll(_message);
                    return true;
                }
            return false;
        }
    }
}