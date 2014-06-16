using System;
using NUnit;
using NUnit.Framework;
using DataInjector;

namespace TestProject
{
    [TestFixture]
    public class IntegrationTest
    {
        [Test]
        public void EndToEndTest()
        {
            var builder = new ReportBuilderService(new DocumentDeconstructService(), new ReportTemplateService(),
                                                   new DocumentReconstructService());
            builder.DoStuff("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 1.odt",
                            new {Date = DateTime.Now.Day, Name = "Fancypants McSnooterson"});
        }
    }
}