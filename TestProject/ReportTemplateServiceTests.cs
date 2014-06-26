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
            //UNDONE: Implement this test fixture!
        }


        //TODO: Rework test fixtures to tests specs rather than methods?


    }
}
