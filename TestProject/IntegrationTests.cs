using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.IO.Compression;
using NUnit.Framework;
using DataInjector;
using ZipDiff.Core.Output;
using ZipDiff.Core;
using ZipDiff;


namespace TestProject
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void CorrectlyCompilesAndParsesBasicTemplate()
        {
            const string basicReportGenerated = "..\\..\\Generated Reports\\Basic Report.odt";

            const string basicReportExpected = "..\\..\\Expected Report Outputs\\Basic Report.odt";
            byte[] templateFile = File.ReadAllBytes("..\\..\\Test Templates\\Basic Template.odt");
            var model = new {Date = DateTime.Now.Day, DateTime.Now.Month, Name = "Fancypants McSnooterson"};
            var reportBuilderService = new ReportBuilderService(new ReportTemplateService(), new DataHandlerService());
            using (
                var report = new FileStream(basicReportGenerated, FileMode.Create)
                )
            {
                reportBuilderService.BuildReport(templateFile, model, report);
            }
            var diffs = GetDifferences(basicReportExpected, basicReportGenerated);
            var hasDifferences = diffs.HasDifferences();
            Assert.That(!hasDifferences);
        }

        [Test]
        public void CorrectlyParsesTemplateWithForeachLoop()
        {
            const string loopReportGenerated = "..\\..\\Expected Report Outputs\\Loop Report.odt";
            const string loopReportExpected = "..\\..\\Generated Reports\\Basic Report.odt";

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

            var reportBuilderService = new ReportBuilderService(new ReportTemplateService(), new DataHandlerService());
            reportBuilderService.BuildReport(File.ReadAllBytes("..\\..\\Test Templates\\Loop Template.odt"), model,
                                             new FileStream("..\\..\\Generated Reports\\Loop Report.odt",
                                                            FileMode.Create));
            var diffs = GetDifferences(loopReportExpected, loopReportGenerated);
            Assert.That(diffs.Changed.Count==5 );
        }

        private static Differences GetDifferences(string path1, string path2)
        {
            var calculator = new DifferenceCalculator(path1, path2);
            var diffs = calculator.GetDifferences();
            return diffs;
        }
    }
}