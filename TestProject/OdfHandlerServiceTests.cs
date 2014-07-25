#region

using System.IO.Compression;
using AntiShaun;
using Moq;
using NUnit.Framework;

#endregion

namespace TestProject
{
	internal class OdfHandlerServiceTests
	{
		private static readonly Mock<IFileHandlerService> Mock = new Mock<IFileHandlerService>();
		private static readonly Mock<IZipHandlerService> MockZipHandler = new Mock<IZipHandlerService>();
		private readonly OdfHandlerService _sut = new OdfHandlerService(Mock.Object, MockZipHandler.Object);
		private readonly Mock<IZipArchive> _zip = new Mock<IZipArchive>();


		[Test]
		public void Builds_Zip_From_Document()
		{
			var document = new byte[0];
			Mock.Setup(x => x.ZipArchiveFromDocument(document, ZipArchiveMode.Read)).Returns(_zip.Object);
			MockZipHandler.Setup(x => x.GetEntryAsString(_zip.Object, string.Empty)).Returns("Xml Metadata");

			_sut.BuildDocumentInformation(document);
			Mock.Verify(x => x.ZipArchiveFromDocument(document, ZipArchiveMode.Read));
		}
	}
}