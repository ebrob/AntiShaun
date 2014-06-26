using DataInjector;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    class FileSystemServiceTests
    {
        private FileSystemService _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new FileSystemService(new DecompressionService());
        }

    }
}
