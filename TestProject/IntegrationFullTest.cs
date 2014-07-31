#region

using System.IO;
using AntiShaun;
using NUnit.Framework;
using ZipDiff.Core;

#endregion

namespace TestProject
{
	public class BasicModel
	{
		public string Name;
	}

	internal class IntegrationFullTest
	{
		[Test]
		public void IntegrationTest()
		{
			const string templatePath = @"..\..\Test Templates\Modeled Basic Template.odt";
			const string reportPath = @"..\..\Generated Reports\Very Basic Report.odt";
			const string expectedReportPath = @"..\..\Expected Report Outputs\Very Basic Report.odt";
			var templateService = new TemplateBuilderService(new TemplateFactory(),
			                                                 new OdfHandlerService(new FileHandlerService(),
			                                                                       new ZipHandlerService(),
			                                                                       new BuildOdfMetadataService(),new XmlNamespaceService(), new IxDocumentParserServiceService()),new XmlNamespaceService());
				//This constructor is terrifying.
			var document = File.ReadAllBytes(templatePath);

			var template = templateService.BuildTemplate(document);

			var compileService = new CompileService();
			compileService.Compile(template, "Template 1");

			var reportService = new ReportGeneratorService(new FileHandlerService());
			using (var report = new FileStream(reportPath, FileMode.Create))
			{
				reportService.BuildReport(template, new BasicModel {Name = "Fancypants McSnooterson"}, report);
			}
			var diffs = GetDifferences(expectedReportPath, reportPath);
			var thereAreDifferences = diffs.HasDifferences();
			Assert.That(!thereAreDifferences);
		}

		private static Differences GetDifferences(string path1, string path2)
		{
			var calculator = new DifferenceCalculator(path1, path2);
			var diffs = calculator.GetDifferences();
			return diffs;
		}
	}
}