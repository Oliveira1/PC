using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie1PC;

namespace TestsSerie1Pc
{
    [TestClass]
    public class TestRendezvousChannel
    {

        [TestMethod]
        public void WithOneClientAndOneServerThreadByRunningServerFirst()
        {   
            var synchronizer = new RendezvousChannel<int,int>();
            var waitingToStart = new ManualResetEvent(false);
            var server=new Thread(() =>
            {
                int service;
                waitingToStart.Set();
                var myToken = (RendezvousChannel<int, int>.Token)synchronizer.Accept(Timeout.Infinite, out service);
                synchronizer.Reply(myToken,myToken.service*2);
            });
         
            server.Start();
                int response;
            waitingToStart.WaitOne();
            if (synchronizer.Request(2, Timeout.Infinite, out response))
            {
                Assert.AreEqual(4,response);
            }
        }

        [TestMethod]
        public void WithOneClientAndOneServerThreadByRunningClientFirst()
        {
            var synchronizer = new RendezvousChannel<int, int>();
            var waitingToStart = new ManualResetEvent(false);
            var waitingToEnd=new ManualResetEvent(false);
            var result = 0;
            var client = new Thread(() =>
            {
               
                 int response; 
                waitingToStart.Set();
                synchronizer.Request(2, Timeout.Infinite, out response);
                result = response;
                waitingToEnd.Set();
            });

            client.Start();
            waitingToStart.WaitOne();
                int service;
                var myToken = (RendezvousChannel<int, int>.Token)synchronizer.Accept(Timeout.Infinite, out service);
                synchronizer.Reply(myToken, myToken.service * 2);
            waitingToEnd.WaitOne();
                Assert.AreEqual(4, result);
            
        }
    }
}
