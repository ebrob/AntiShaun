using System.IO;
using AntiShaun;
using NUnit.Framework;

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
			var templateService = new TemplateBuilderService(new TemplateFactory(), new OdfHandlerService());
			var document = File.ReadAllBytes(@"..\..\Test Templates\Modeled Basic Template.odt");

			var template = templateService.BuildTemplate(document);

			var compileService = new CompileService();
			compileService.Compile(template, "Template 1");

			var reportService = new ReportGeneratorService(new OdfHandlerService());
			reportService.BuildReport(template, new BasicModel {Name = "Fancypants McSnooterson"},
			                          new FileStream(@"..\..\Generated Reports\Very Basic Report.odt", FileMode.Create));
		}
	}
}