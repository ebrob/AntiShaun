using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataInjector;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            var s = ReportBuilder.Main(path: "C:\\Users\\Calvin\\Documents\\TestingBed\\Test 1.odt");
            Console.WriteLine(s);
        }
    }
}