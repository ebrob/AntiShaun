using System;
using System.IO;
using System.Xml;
using DataInjector;
using NUnit.Framework;

namespace TestProject
{

    [TestFixture]
    public class DataInjectionFixture
    {
        private readonly object _model = new {Date = DateTime.Today, Name = "Fancypants McSnooterson"};
        private const string Path = "C:\\Users\\Calvin\\Documents\\TestingBed\\Test 1.odt";

        [Test]
        public void CanCreateValidOdtFile()
        {//TODO: Find better way to make this test
            var reportPath = ReportBuilder.Main(Path, _model);
            var generatedFile = File.Open(reportPath, FileMode.Open);
            var generatedHash = generatedFile.GetHashCode();
            //We assume that a file with a hash identical to an existing valid file is also a valid file. This is a bad assumption. 
            const int expectedHash = 38201556;
            Assert.AreEqual(generatedHash, expectedHash);
        }

        [Test]
        public void CanPerformFirstXmlTransform()
        {
            var expected = new FileStream("C:\\Users\\Calvin\\Documents\\TestingBed\\content_with_no_at_signs.xml",
                                          FileMode.Open);

            var actual = DataTools.EncodeAtSigns("C:\\Users\\Calvin\\Documents\\TestingBed\\content_with_at_signs.xml");

            Assert.AreEqual(expected, actual);
            expected.Dispose();
            actual.Dispose();
        }

        [Test]
        public void CanPerformSelectiveXmlTransform()
        {
            var expectedStream =
                new FileStream(
                    "C:\\Users\\Calvin\\Documents\\TestingBed\\content_with_only_razor_tags.xml",
                    FileMode.Open);
            var doc = new XmlDocument();
            doc.Load(expectedStream);

            var expected = doc.OuterXml;

            var stream = new FileStream("C:\\Users\\Calvin\\Documents\\TestingBed\\content_with_no_at_signs.xml",
                                        FileMode.Open);
            var actual = DataTools.DetectAndConvertTemplateTags(stream);

            Assert.AreEqual(expected, actual);
            expectedStream.Dispose();
            stream.Dispose();
        }


        [Test]
        public void CanParseWithRazorAndFormatFinalResultCorrectly()
        {
            string expected = File.ReadAllText("C:\\Users\\Calvin\\Documents\\TestingBed\\content_final_output.xml");
            Assert.AreEqual(expected,
                            DataTools.RazorAndReinsertAtSigns(
                                File.ReadAllText(
                                    "C:\\Users\\Calvin\\Documents\\TestingBed\\content_with_only_razor_tags.xml"),
                                _model));
        }
    }
}