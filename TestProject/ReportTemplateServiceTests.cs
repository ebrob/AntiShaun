using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataInjector;
using NUnit.Framework;

namespace TestProject

{
    
    [TestFixture]
    class ReportTemplateServiceTests
    {
        private ReportTemplateService _sut;
        private XDocument _document = new XDocument();
        
        [SetUp]
        public void Setup()
        {//instantiate XML Document
         //Figure out how to mock a filesystem   
        }


        //TODO: Rework test fixtures to tests specs rather than methods?


    }
}
