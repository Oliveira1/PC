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
        public void WithOneClientAndOneServerThread()
        {   
            var synchronizer = new RendezvousChannel<int,int>();
            var waitingToStart = new ManualResetEvent(false);
            var server=new Thread(() =>
            {
                int service;
                waitingToStart.Set();
                var myToken=(RendezvousChannel<int,int>.Token)synchronizer.Accept(0,out service);
                synchronizer.Reply(myToken,myToken.service*2);
            });
         
            server.Start();
                int response;
            waitingToStart.WaitOne();
            if (synchronizer.Request(2, 0, out response))
            {
                Assert.AreEqual(4,response);
            }
        }
    }
}
