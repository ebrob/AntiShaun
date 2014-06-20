using System;
using System.Collections.Generic;
using System.Dynamic;
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
            var builder = new ReportBuilderService(new DocumentDeconstructService(new FileSystemService(new DecompressionService())), new ReportTemplateService(),
                                                   new DocumentReconstructService());
            builder.InjectData("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 1.odt",
                            new {Date = DateTime.Now.Day, Name = "Fancypants McSnooterson"});
        }

        [Test]
        public void CorrectlyParsesTemplateWithForeachLoop()
        {
            var builder = new ReportBuilderService(new DocumentDeconstructService(new FileSystemService(new DecompressionService())), new ReportTemplateService(),
                                                   new DocumentReconstructService());
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
            builder.InjectData("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 2.odt", model);
        }


        [Test]
        [ExpectedException]
        public void BreaksOnBadModel()
        {
            var builder = new ReportBuilderService(new DocumentDeconstructService(new FileSystemService(new DecompressionService())), new ReportTemplateService(),
                                                   new DocumentReconstructService());

                            builder.InjectData("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 3.odt",
                            new {});
     

        }

        [Test]
        public void HandlesConditionals()
        {
            var builder = new ReportBuilderService(new DocumentDeconstructService(new FileSystemService(new DecompressionService())), new ReportTemplateService(),
                                                   new DocumentReconstructService());
            builder.InjectData("C:\\Users\\Calvin\\Documents\\TestingBed\\Test 4.odt",
                            new {Variable = 0, Var1 = "Var1"});
        }
    }
    }
