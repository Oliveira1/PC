﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serie1PC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestsSerie1Pc
{
    [TestClass]
    public class TestLogger
    {
        [TestMethod]
        public void TestWriteTo()
        {
            const int MAX_PROCESSORS = 8;
            using (TextWriter write = File.CreateText("./logger.txt"))
            {
                
                var synchronizer = new Logger(write);
                
                    var producer = new Thread(() =>
                    {
                        for (int i = 0; i < MAX_PROCESSORS; i++)
                        {
                            synchronizer.LogMessage("MY ID: " + Thread.CurrentThread.Name + "Cycle number :"+i+"\n");
                        }
                    });
                    producer.Start();

                producer.Join();
                synchronizer.Start();
                synchronizer.Stop();

            }
        }
 
       
    }
}