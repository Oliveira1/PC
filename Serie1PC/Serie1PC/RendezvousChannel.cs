using System.Collections.Generic;
using System.Threading;


namespace Serie1PC
{
    public class RendezvousChannel<S,R>
    {
        public class Token
        {
            public S service;

            public R response;

            public Token(S service, R Response)
            {
                this.service = service;
                this.response = response;
            }

            public Token(S service)
            {
                this.service = service;
                this.response = default(R);
            }

            public Token(R Response)
            {
                this.service = default(S);
                this.response = response;
            }

            public Token()
            {
                service = default(S);
                response = default(R);
            }
        }

        private class Ticket
        {
            public  Token token;
            internal Status processing;

            public Ticket()
            {
                token = new Token();
                processing = Status.Open;
            }
        }
        private LinkedList<Ticket> _services;
        private LinkedList<Ticket> _processingRequests;
        public enum Status{Open,Accepting, InProcess, Concluded }
        public RendezvousChannel()
        {
            _services=new LinkedList<Ticket>();
            _processingRequests = new LinkedList<Ticket>();
        }
        public bool Request(S service,int timeout, out R response)
        {
            lock (this)
            {
                Ticket myTicket;
                if(CheckForAcceptingTokens(out myTicket,service))
                {
                   Monitor.PulseAll(this);
                }
                else
                {
                    myTicket.token=new Token(service);
                     _services.AddLast(myTicket);
                }
                    
                while (true)
                {
                    Monitor.Wait(this);
                    if (myTicket.processing == Status.Concluded)
                    {
                        response = myTicket.token.response;
                        return true;
                    }
                }
            }
        }
        private bool CheckForAcceptingTokens(out Ticket myTicket, S service)
        {
            foreach (var ticket in _services)
            {
                if (ticket.processing == Status.Accepting)
                {
                    ticket.token.service = service;
                    myTicket = ticket;
                    ticket.processing=Status.InProcess;
                    _services.Remove(ticket);
                    _processingRequests.AddLast(ticket);
                    return true;
                }
            }
            myTicket=new Ticket();
            return false;
        }

        public  object Accept(int timeout, out S service)
    {
        lock (this)
        {
            var myTicket = new Ticket();
            if (_services.Count !=0)
            {
                RemoveFirstRequestedToken(ref myTicket);
                Monitor.PulseAll(this);   // modificar, para já é sem notificação específica de Threads
                service = myTicket.token.service;
                return myTicket.token;
            }
            myTicket.processing = Status.Accepting;
            _services.AddLast(myTicket);
            while (true)
            {
                Monitor.Wait(this);
                if (myTicket.processing==Status.InProcess)
                {
                    service = myTicket.token.service;
                    return myTicket.token;
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
        private void RemoveFirstRequestedToken(ref Ticket myTicket)
        {
            foreach (var ticket in _services)
            {
                if (ticket.processing == Status.Open)
                {
                    myTicket = ticket;
                    ticket.processing = Status.InProcess;
                    _services.Remove(ticket);
                    _processingRequests.AddLast(ticket);
                    return;
                }

            }
            
        }
        private void FindTokenInProcessingRequests(ref Token myToken, R response)
        {

            foreach (var ticket in _processingRequests)
            {
                if (ticket.token.Equals(myToken))
                {
                    myToken.response = response;
                    ticket.processing = Status.Concluded;
                    _processingRequests.Remove(ticket);
                    return;
                }
            }
        }
    }
}
