using System;
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
            if(userName==null) Assert.Fail("null UserName");
            String s = "Searching on C:\\Users\\" + userName + "\\Downloads\\";
            Console.WriteLine(s);
            int i = Exercicio3.Find_By_Sequence("C:\\Users\\" + userName + "\\Downloads\\", "*.zip", "*");
            Console.WriteLine(i);

        }
    }
}
