using System;
using System.IO;
using DataInjector;
using Moq;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    internal class DeconstructServiceTests
    {
        private DocumentDeconstructService _sut;
        private readonly object _model = new {dog = "Spot"};
        private readonly string Path = "c:\\spot\\run";
        private Mock<IFileSystemService> _mockFileService;

        [SetUp]
        public void TestSetup()
        {
            _mockFileService = new Mock<IFileSystemService>();
            _sut = new DocumentDeconstructService();

        }




    }
}