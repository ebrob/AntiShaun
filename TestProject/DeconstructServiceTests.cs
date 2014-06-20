using DataInjector;
using Moq;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    internal class DeconstructServiceTests
    {
        private DocumentDeconstructService _sut;
        private const string Path = "c:\\spot\\run";
        private Mock<IFileSystemService> _mockFileService;


        [SetUp]
        public void TestSetup()
        {
            _mockFileService = new Mock<IFileSystemService>();

            _sut = new DocumentDeconstructService(_mockFileService.Object);
        }

        [Test]
        public void UnzipsFile()
        {
            _mockFileService.Setup(service => service.UnzipAndGetContentPath(Path));
            _sut.UnzipAndGetContent(Path);
            _mockFileService.Verify(service => service.UnzipAndGetContentPath(Path));
        }

        [Test]
        public void GetsTempFolderPath()
        {
            _mockFileService.Setup(service => service.UnzipAndGetContentPath(Path));
            _mockFileService.Setup(service => service.TempFolderPath);
            _sut.UnzipAndGetContent(Path);
            _mockFileService.Verify(service => service.UnzipAndGetContentPath(Path));
            _mockFileService.Verify(service => service.TempFolderPath);
        }

        [Test]
        public void GetsContentString()
        {
            _mockFileService.Setup(service => service.UnzipAndGetContentPath(Path)).Returns("fakeContentPath");
            _mockFileService.Setup(service => service.TempFolderPath);
            _mockFileService.Setup(service => service.GetFileContentAsString("fakeContentPath"));
            _sut.UnzipAndGetContent(Path);
            _mockFileService.Verify(service => service.UnzipAndGetContentPath(Path));
            _mockFileService.Verify(service => service.TempFolderPath);
            _mockFileService.Verify(service => service.GetFileContentAsString("fakeContentPath"));
        }
    }
}