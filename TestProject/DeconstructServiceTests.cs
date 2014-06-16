using System;
using System.IO;
using DataInjector;
using NUnit.Framework;

namespace TestProject
{
    [TestFixture]
    public class DeconstructServiceTests
    {
        [Test]
        public void DeconstructorReturnsFilePathTest()
        {
            const string path = "C:\\Users\\Calvin\\Documents\\TestingBed\\Test 1.odt";

            using (var deconstructor = new DocumentDeconstructService())
            {
                deconstructor.Deconstruct(path);
                Assert.That(Directory.Exists(deconstructor.TempFolderPath));
            }
        }
    }
}
