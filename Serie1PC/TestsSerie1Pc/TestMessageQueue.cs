using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie1PC;

namespace TestsSerie1Pc
{
    [TestClass]
    public class TestMessageQueue
    {
        public class Message<T>
        {
            public Predicate<uint> pred;
            public uint value;
            public T msg;
            public Boolean taken;
        } 

        [TestMethod]
        public void InsertAndRemoveFromMessageWithTwoThreads()
        {
            var synchronizer=new MessageQueue<int>(10);
            var producer =new Thread(() =>
            {
               var myMsg=new Serie1PC.Message<int>();
                myMsg.type = 1;
                myMsg.content = 2;
                synchronizer.Send(myMsg);
            });
            producer.Start();
            producer.Join();
           var x= synchronizer.Receive(CompareEquals);
            Assert.AreEqual(x, 2);
        }

        [TestMethod]
        public void ReturningFromReceiveQueue()
        {
            var synchronizer = new MessageQueue<int>(0);
            const int MAX_PROCESSORS=8;
            var waitingToStart = new CountdownEvent(MAX_PROCESSORS);
            var waitingToEnd = new CountdownEvent(MAX_PROCESSORS);
            var threadsResultCounter = 0;
            for (int i = 0; i < MAX_PROCESSORS; i++)
            {
                var consumer = new Thread(()=>
                {
                    waitingToStart.Signal();
                    int result = synchronizer.Receive(CompareEquals);
                    if (result == 1)
                    {
                        Interlocked.Increment(ref threadsResultCounter);
                    }
                    waitingToEnd.Signal();
                });    
                consumer.Start();
            }

            waitingToStart.Wait();
            var myMsg = new Serie1PC.Message<int>();
            for(int i=0;i<MAX_PROCESSORS;i++){
                myMsg.type =1;
                myMsg.content = 1;
                synchronizer.Send(myMsg);  
            }
            waitingToEnd.Wait();
            Assert.AreEqual(threadsResultCounter, MAX_PROCESSORS);
        }

        [TestMethod]
        public void ReturningFromSendQueue()
        {
            var synchronizer = new MessageQueue<int>(0);
            const int MAX_PROCESSORS = 8;
            var waitingToStart = new CountdownEvent(MAX_PROCESSORS);
            var waitingToEnd = new CountdownEvent(MAX_PROCESSORS);

            for (var i = 0; i < MAX_PROCESSORS; i++)
            {
                var consumer = new Thread(() =>
                {
                    var myMsg = new Serie1PC.Message<int>();
                    myMsg.type = 1;
                    myMsg.content = 1;
                    waitingToStart.Signal();
                   synchronizer.Send(myMsg);
                //  waitingToEnd.Signal();
                });
                consumer.Start();
            }

            waitingToStart.Wait();
            for (var i = 0; i < MAX_PROCESSORS; i++)
            {
                var result=synchronizer.Receive(CompareEquals);
                Assert.AreEqual(result,1);
            }
         //   waitingToEnd.Wait();
        }

        private Boolean CompareEquals( uint value)
        {
            return value==1;
        }
    }


}
