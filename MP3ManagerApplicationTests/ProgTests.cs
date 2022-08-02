using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MP3ManagerApplication.Tests
{
    [TestClass()]
    public class ProgTests
    {
        [TestMethod()]
        public void calcPercentageTest()
        {
            Assert.AreEqual(1, Prog.calcPercentage(1, 1));
            Assert.AreEqual(0.5, Prog.calcPercentage(1, 2));

            Console.WriteLine("{0:P1}", Prog.calcPercentage(1, 1));
        }
    }
}