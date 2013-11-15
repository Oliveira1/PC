using System;
using System.Collections.Generic;
using System.Threading;

namespace Serie1PC
{
    public class RendezvousChannel<S, R>
    {
        public enum Status
        {
            Open,
            Accepting,
            InProcess,
            Concluded
        };

        private readonly Object _lock = new Object();
        private readonly LinkedList<Ticket> _processingRequests;
        private readonly LinkedList<Ticket> _services;

        public RendezvousChannel()
        {
            _services = new LinkedList<Ticket>();
            _processingRequests = new LinkedList<Ticket>();
        }

        public bool Request(S service, int timeout, out R response)
        {
            lock (_lock)
            {
                Ticket myTicket;
                if (CheckForAcceptingTokens(out myTicket, service))
                {
                    SyncUtils.Notify(_lock, myTicket._condition);
                }
                else
                {
                    myTicket.token = new Token(service);
                    myTicket._condition = new Object();

                    _services.AddLast(myTicket);
                }

                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
                while (true)
                {
                    try
                    {
                        SyncUtils.Wait(_lock, myTicket._condition, timeout);
                        if (myTicket.processing == Status.Concluded)
                        {
                            response = myTicket.token.response;
                            return true;
                        }

                        if (SyncUtils.AdjustTimeout(ref lastTime, ref timeout) == 0 &&
                            myTicket.processing == Status.Open)
                        {
                            _services.Remove(myTicket);
                            response = default(R);
                            return false;
                        }
                    }
                    catch (ThreadInterruptedException e)
                    {
                        if (myTicket.processing == Status.Open)
                        {
                            Thread.CurrentThread.Interrupt();
                            _services.Remove(myTicket);
                            response = default(R);
                            return false;
                        }
                        if (myTicket.processing == Status.Concluded)
                        {
                            response = myTicket.token.response;
                            return true;
                        }
                        timeout = Timeout.Infinite;
                    }
                }
            }
        }

        private bool CheckForAcceptingTokens(out Ticket myTicket, S service)
        {
            foreach (Ticket ticket in _services)
            {
                if (ticket.processing == Status.Accepting)
                {
                    ticket.token.service = service;
                    myTicket = ticket;
                    ticket.processing = Status.InProcess;
                    _services.Remove(ticket);
                    _processingRequests.AddLast(ticket);
                    return true;
                }
            }
            myTicket = new Ticket();
            return false;
        }

        public object Accept(int timeout, out S service)
        {
            lock (_lock)
            {
                var myTicket = new Ticket();
                if (_services.Count != 0)
                {
                    RemoveFirstRequestedToken(ref myTicket);
                    service = myTicket.token.service;
                    var token = myTicket.token;
                    return token;
                }

                myTicket._condition = new Object();
                myTicket.processing = Status.Accepting;
                _services.AddLast(myTicket);
                int lastTime = (timeout != Timeout.Infinite) ? Environment.TickCount : 0;
                while (true)
                {
                    SyncUtils.Wait(_lock, myTicket._condition, timeout);
                    if (myTicket.processing == Status.InProcess)
                    {
                        service = myTicket.token.service;
                        return myTicket.token;
                    }
                    if (SyncUtils.AdjustTimeout(ref lastTime, ref timeout) == 0)
                    {
                        _services.Remove(myTicket);
                        service = default(S);
                        return null;
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
            foreach (Ticket ticket in _processingRequests)
            {
                if (ticket.token.Equals(myToken))
                {
                    myToken.response = response;
                    ticket.processing = Status.Concluded;
                    _processingRequests.Remove(ticket);
                    SyncUtils.Notify(_lock, ticket._condition);
                    return;
                }
            }
        }

        private class Ticket
        {
            public object _condition;
            internal Status processing;
            public Token token;

            public Ticket()
            {
                token = new Token();
                processing = Status.Open;
            }
        }

        public class Token
        {
            public R response;
            public S service;

            public Token(S service)
            {
                this.service = service;
                response = default(R);
            }


            public Token()
            {
                service = default(S);
                response = default(R);
            }
        }
    }
}