using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFilesSearch;

namespace Serie3Tests
{
    [TestClass]
    public class Exercicio3Tests
    {
        public Exercicio3Tests(CancellationTokenSource cts)
        {
            _cts = cts;
        }

        private CancellationTokenSource _cts;

        [TestMethod]
        public void Test_C_USERS_DOWNLOADS_ALL()
        {
          /*  // if null throws nullPointerException;
            string userName = Environment.UserName;
            if (userName == null) Assert.Fail("null UserName");
            String s = "Searching on C:\\Users\\" + userName + "\\Downloads\\";
            Console.WriteLine(s);
            IEnumerable<Exercicio3.SearchResult> x = Exercicio3.Find_By_Sequence("C:\\Users\\" + userName + "\\Downloads\\", "*.zip",
                "a", _cts);

            foreach (var i in x)
            {
            Console.WriteLine("total:" + i.total + "\n Found matches:" + i.matching_sequence);
            foreach (var path in i.paths)
                Console.WriteLine(path);
                
            }*/
        }
    }
}