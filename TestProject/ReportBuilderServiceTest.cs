using System;
using DataInjector;
using Moq;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    internal class ReportBuilderServiceTest
    {
        private ReportBuilderService _sut;
        private Mock<IDocumentReconstructService> _mockReconstruct;
        private Mock<IDocumentDeconstructService> _mockDeconstruct;
        private Mock<IReportTemplateService> _mockTemplate;
        private readonly object _model = new {dog = "Spot"};
        private readonly string Path = "c:\\spot\\run";


        [SetUp]
        public void SetUpEachTest()
        {
            _mockDeconstruct = new Mock<IDocumentDeconstructService>();
            _mockReconstruct = new Mock<IDocumentReconstructService>();
            _mockTemplate = new Mock<IReportTemplateService>();
            _sut = new ReportBuilderService(_mockDeconstruct.Object, _mockTemplate.Object, _mockReconstruct.Object);
        }

        [Test]
        public void DeconstructsDocument()
        {
            _mockDeconstruct.Setup(x => x.Deconstruct("c:\\spot\\run"));
            _sut.DoStuff(Path, _model);
            _mockDeconstruct.Verify(x => x.Deconstruct("c:\\spot\\run"));
        }

        [Test]
        public void TemplatesDocument()
        {
            _mockDeconstruct.Setup(x => x.Deconstruct("c:\\spot\\run")).Returns("FakeXmlContent");
            _mockTemplate.Setup(x => x.ApplyTemplate("FakeXmlContent", _model));
            _sut.DoStuff(Path, _model);
            _mockTemplate.Verify(x => x.ApplyTemplate("FakeXmlContent", _model));
            _mockDeconstruct.Verify(x => x.Deconstruct("c:\\spot\\run"));
        }

        [Test]
        public void ReconstructsDocument()
        {
            _mockDeconstruct.SetupGet(x => x.TempFolderPath).Returns("FakeTempPath");
            _mockDeconstruct.Setup(x => x.Deconstruct("c:\\spot\\run")).Returns("FakeXmlContent");
            _mockTemplate.Setup(x => x.ApplyTemplate("FakeXmlContent", _model)).Returns("FakeTransformedContent");
            _mockReconstruct.Setup(x => x.Reconstruct("FakeTempPath", "FakeTransformedContent"));

            _sut.DoStuff(Path, _model);

            _mockDeconstruct.Verify(x => x.Deconstruct("c:\\spot\\run"));
            _mockTemplate.Verify(x => x.ApplyTemplate("FakeXmlContent", _model));
            _mockReconstruct.Verify(x => x.Reconstruct("FakeTempPath", "FakeTransformedContent"));
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void RejectsNullPath()
        {
            _sut.DoStuff(null, _model);
        }

        [Test]
        [ExpectedException(typeof (ArgumentException))]
        public void RejectsEmptyPath()
        {
            _sut.DoStuff(string.Empty, _model);
        }
    }
}