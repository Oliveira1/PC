using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serie1PC;
namespace TestsSerie1Pc
{
    [TestClass]
    public class TestCompletion
    {
        [TestMethod]
        public void TestCompleteAndWaitMethodWithTwoThreads()
        {
            var synchronizer = new Completion();
            var producer = new Thread(synchronizer.WaitForCompletion);
            producer.Start();
            synchronizer.Complete();
            producer.Join();
        }

        [TestMethod]
        public void TestCompleteAllMethod()
        {
            const int MAX_PROCESSORS = 8;
            var synchronizer = new Completion();
            var waitingForAllThreadsToStart= new CountdownEvent(MAX_PROCESSORS);
            var waitingForAllThreadsToEnd = new CountdownEvent(MAX_PROCESSORS);
            for (var i = 0; i < MAX_PROCESSORS; i++)
            {    
               var producer = new Thread(() =>
                {
                    waitingForAllThreadsToStart.Signal();
                    waitingForAllThreadsToStart.Wait();
                    synchronizer.WaitForCompletion();
                    waitingForAllThreadsToEnd.Signal();

                });
                producer.Start();
            }
            synchronizer.CompleteAll();
            waitingForAllThreadsToEnd.Wait();
            waitingForAllThreadsToStart.Reset();
            waitingForAllThreadsToEnd.Reset();
            for (var i = 0; i < MAX_PROCESSORS; i++)
            {
                var producer = new Thread(() =>
                {
                    waitingForAllThreadsToStart.Signal();
                    waitingForAllThreadsToStart.Wait();
                    synchronizer.WaitForCompletion();
                    waitingForAllThreadsToEnd.Signal();

                });
                producer.Start();
            }

            waitingForAllThreadsToEnd.Wait();

        }



    }
}
