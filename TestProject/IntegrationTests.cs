using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Abstractions;
using NUnit.Framework;
using DataInjector;


namespace TestProject
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void CorrectlyCompilesAndParsesBasicTemplate()
        {
            byte[] fileBytes = File.ReadAllBytes("..\\..\\TestProject\\Basic Template.odt");
            var model = new {Date = DateTime.Now.Day, DateTime.Now.Month, Name = "Fancypants McSnooterson"};

        }

        [Test]
        public void CorrectlyParsesTemplateWithForeachLoop()
        {//TODO: This is busted
            dynamic model = new ExpandoObject();

            dynamic client1 = new ExpandoObject();
            client1.FirstName = "FancyPants";
            client1.Lastname = "McSnooterson";
            dynamic client2 = new ExpandoObject();
            client2.FirstName = "Bob";
            client2.Lastname = "Jones";


            model.Clients = new List<dynamic>();
            model.Clients.Add(client1);
            model.Clients.Add(client2);
        }
    }
}