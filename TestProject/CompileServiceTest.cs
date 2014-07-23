using AntiShaun;
using Moq;
using NUnit.Framework;
using RazorEngine.Templating;

namespace TestProject
{
	internal class CompileServiceTest
	{
		private readonly Mock<CompileService> _sut = new Mock<CompileService>();
		private readonly Mock<Template> _template = new Mock<Template>();
		private const string TemplateName = "testTemplate";
		private const string Template = "SomeTemplate";

		[Test]
		public void RazorEngineCompileTest()
		{
			var razorEngineTemplateService = new TemplateService();
			if (razorEngineTemplateService.HasTemplate(TemplateName))
			{
				razorEngineTemplateService.RemoveTemplate(TemplateName);
			}
			_template.SetupGet(x => x.Content).Returns(Template);
			_sut.Object.Compile(_template.Object, TemplateName);

			Assert.That(razorEngineTemplateService.HasTemplate(TemplateName));
		}
	}
}