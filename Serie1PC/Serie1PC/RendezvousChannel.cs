using System.Collections.Generic;
using System.Threading;


namespace Serie1PC
{
    public class RendezvousChannel<S,R>
    {
        public class Token
        {
            public S service;
            public Status processing=Status.Open;
            public R response;
        }
        private LinkedList<Token> _services;
        private LinkedList<Token> _processingRequests;
        public enum Status{Open,Accepting, InProcess, Concluded }
        public RendezvousChannel()
        {
            _services=new LinkedList<Token>();
            _processingRequests = new LinkedList<Token>();
        }
        public bool Request(S service,int timeout, out R response)
        {
            lock (this)
            {
                Token myToken;
                if(CheckForAcceptingTokens(out myToken,service))
                {
                   Monitor.PulseAll(this);
                }
                else
                {
                     myToken.service = service;
                     _services.AddLast(myToken);
                }
                    
                while (true)
                {
                    Monitor.Wait(this);
                    if (myToken.processing == Status.Concluded)
                    {
                        response = myToken.response;
                        return true;
                    }
                }
            }
        }
        private bool CheckForAcceptingTokens(out Token myToken, S service)
        {
            foreach (var token in _services)
            {
                if (token.processing == Status.Accepting)
                {
                    myToken = token;
                    myToken.service = service;
                    myToken.processing=Status.InProcess;
                    _services.Remove(token);
                    _processingRequests.AddLast(token);
                    return true;
                }
            }
            myToken=new Token();
            return false;
        }

        public  object Accept(int timeout, out S service)
    {
        lock (this)
        {
            var myToken=new Token();
            if (_services.Count !=0)
            {
                RemoveFirstRequestedToken(ref myToken);
                Monitor.PulseAll(this);   // modificar, para já é sem notificação específica de Threads
                service = myToken.service;
                return myToken;
            }
            myToken.processing = Status.Accepting;
            _services.AddLast(myToken);
            while (true)
            {
                Monitor.Wait(this);
                if (myToken.processing==Status.InProcess)
                {
                    service = myToken.service;
                    return myToken;
                }
            }
        }

    }
    public void Reply(object rendezVousToken, R Response)
        {
            Token myToken;
            lock (this)
            {
                myToken = (Token) rendezVousToken;
                FindTokenInProcessingRequests(ref myToken,Response);
                Monitor.PulseAll(this);
            }
        }
        private void RemoveFirstRequestedToken(ref Token myToken)
        {
            foreach (var token in _services)
            {
                if (token.processing == Status.Open)
                {
                    myToken = token;
                    myToken.processing = Status.InProcess;
                    _services.Remove(token);
                    _processingRequests.AddLast(token);
                    return;
                }

            }
            
        }
        private void FindTokenInProcessingRequests(ref Token myToken, R response)
        {

                var linkedListNode = _processingRequests.Find(myToken);
                if (linkedListNode != null)
                {
                    Token request = linkedListNode.Value;
                    request.processing = Status.Concluded;
                    request.response = response;
                    _processingRequests.Remove(request);
                }
        }
    }
}
