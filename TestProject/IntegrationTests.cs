using System;
using System.Collections.Generic;
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
            var builder = new ReportBuilderService(new DocumentDeconstructService(), new ReportTemplateService(),
                                                   new DocumentReconstructService());
            builder.DoStuff("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 1.odt",
                            new {Date = DateTime.Now.Day, Name = "Fancypants McSnooterson"});
        }

        [Test]
        [ExpectedException]
        public void BreaksOnBadModel()
        {
            var builder = new ReportBuilderService(new DocumentDeconstructService(), new ReportTemplateService(),
                                                   new DocumentReconstructService());
            builder.DoStuff("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 2.odt",
                            new {Date = DateTime.Now.Day, Name = "Fancypants McSnooterson"});
        }


        [Test]
        public void CorrectlyParsesTemplateWithForeachLoop()
        {
            var builder = new ReportBuilderService(new DocumentDeconstructService(), new ReportTemplateService(),
                                                   new DocumentReconstructService());
            builder.DoStuff("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 3.odt",
                            new {});
        }


    }
} 