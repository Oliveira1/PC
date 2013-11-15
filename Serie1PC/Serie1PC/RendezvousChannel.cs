using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;



namespace Serie1PC
{
    public class RendezvousChannel<S,R>
    {
        
        public class Token
        {
            public S service;
            public R response;

            public Token(S service)
            {
                this.service = service;
                this.response = default(R);
            }

      

            public Token()
            {
                service = default(S);
                response = default(R);
            }
        }
        private class Ticket
        {
            public Token token;
            internal Status processing;
            public object _condition;

            public Ticket()
            {
                token = new Token();
                processing = Status.Open;
            }
        }

        private readonly Object _lock = new Object();
        private readonly LinkedList<Ticket> _services;
        private readonly LinkedList<Ticket> _processingRequests;

            public enum Status
            {
                Open,
                Accepting,
                InProcess,
                Concluded
            };
        public RendezvousChannel()
        {
            _services=new LinkedList<Ticket>();
            _processingRequests = new LinkedList<Ticket>();
        }

        public bool Request(S service,int timeout, out R response)
        {
           lock(_lock){
                    Ticket myTicket;
                    if (CheckForAcceptingTokens(out myTicket, service))
                    {
                     SyncUtils.Notify(_lock,myTicket._condition);
                    }
                    else
                    {
                          myTicket.token = new Token(service);
                        myTicket._condition = new Object();
                        _services.AddLast(myTicket);
                    }
                  
                    while (true)
                    {
                        SyncUtils.Wait(_lock,myTicket._condition);
                        if (myTicket.processing == Status.Concluded)
                        {
                            response = myTicket.token.response;
                            return true;
                        }
                    }
            }
            
        }
        private bool CheckForAcceptingTokens( out Ticket myTicket, S service)
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
            lock (_lock)
            {
                    var myTicket = new Ticket();
                    Token token = new Token();
                    if (_services.Count != 0)
                    {
                        RemoveFirstRequestedToken(ref myTicket);
                        service = myTicket.token.service;
                        token = myTicket.token;
                        return token;
                    }

                    myTicket._condition = new Object();
                    myTicket.processing = Status.Accepting;
                    _services.AddLast(myTicket);
                    

                    while (true)
                    {
                        SyncUtils.Wait(_lock, myTicket._condition);
                        if (myTicket.processing == Status.InProcess)
                        {
                            service = myTicket.token.service;
                            return myTicket.token;
                        }
                    }
          }
    }
    public void Reply(object rendezVousToken, R Response)
        {
        lock (_lock)
        {
                var myToken = (Token) rendezVousToken;
                FindTokenInProcessingRequests(ref myToken, Response);       
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
                    SyncUtils.Notify(_lock,ticket._condition);           
                    return;
                }
            }
        }
    }
}
