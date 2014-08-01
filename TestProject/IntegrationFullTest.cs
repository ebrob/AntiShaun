/*Copyright 2014 EventBooking.com, LLC

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. 
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, 
software distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and limitations under the License
*/
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