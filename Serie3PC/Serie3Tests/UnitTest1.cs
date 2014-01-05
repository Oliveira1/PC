using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyServer;

namespace Serie3Tests
{
    [TestClass]
    public class Exercicio3Tests
    {
        [TestMethod]
        public void Test_C_USERS_DOWNLOADS_ALL()
        {
            // if null throws nullPointerException;
            string userName = Environment.UserName;
            if (userName == null) Assert.Fail("null UserName");
            String s = "Searching on C:\\Users\\" + userName + "\\Downloads\\";
            Console.WriteLine(s);
            Exercicio3.SearchResult i = Exercicio3.Find_By_Sequence("C:\\Users\\" + userName + "\\Downloads\\", "*.zip",
                "a");

            Console.WriteLine("total:" + i.total + "\n Found matches:" + i.matching_sequence);
            foreach (var path in i.paths)
                Console.WriteLine(path);
        }
    }
}