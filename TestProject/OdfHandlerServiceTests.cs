#region Apache License

// /*Copyright 2014 EventBooking.com, LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and limitations under the License
// */

#endregion

#region

using System.IO.Compression;
using System.Xml;
using AntiShaun;
using Moq;
using NUnit.Framework;

#endregion

namespace TestProject
{
	internal class OdfHandlerServiceTests
	{
		private const string Content = "content.xml";
		private const string Meta = "meta.xml";
		private const string XmlMetadata = "Xml Metadata";
		private static readonly Mock<IZipFactory> Mock = new Mock<IZipFactory>();
		private static readonly Mock<IZipHandlerService> MockZipHandler = new Mock<IZipHandlerService>();
		private static readonly Mock<IBuildOdfMetadataService> MockMetadataService = new Mock<IBuildOdfMetadataService>();
		private static readonly Mock<IXmlNamespaceResolver> MockXmlNamespaceService = new Mock<IXmlNamespaceResolver>();

		private static readonly Mock<IXDocumentParserService> MockXDocumentParser =
			new Mock<IXDocumentParserService>();

		private readonly byte[] _document = new byte[0];

		private readonly OdfHandlerService _sut = new OdfHandlerService( Mock.Object, MockZipHandler.Object,
		                                                                 MockMetadataService.Object,
		                                                                 MockXmlNamespaceService.Object,
		                                                                 MockXDocumentParser.Object );


		private readonly Mock<IZipArchive> _zip = new Mock<IZipArchive>();


		[SetUp]
		public void SetUp ()
		{
			Mock.Setup( x => x.ZipArchiveFromDocument( _document, ZipArchiveMode.Read ) ).Returns( _zip.Object );
		}

		[Test]
		public void Builds_Zip_From_Document ()
		{
			_sut.BuildDocumentInformation( _document );

			Mock.Verify( x => x.ZipArchiveFromDocument( _document, ZipArchiveMode.Read ) );
		}

		[Test]
		public void Gets_File_Type ()
		{
			MockZipHandler.Setup( x => x.GetFileType( _zip.Object ) );

			_sut.BuildDocumentInformation( _document );

			MockZipHandler.Verify( x => x.GetFileType( _zip.Object ) );
		}

		[Test]
		public void Gets_Content_Entry ()
		{
			MockZipHandler.Setup( x => x.GetEntryAsString( _zip.Object, Content ) );

			_sut.BuildDocumentInformation( _document );

			MockZipHandler.Verify( x => x.GetEntryAsString( _zip.Object, Content ) );
		}

		[Test]
		public void Gets_Metadata_Entry ()
		{
			MockZipHandler.Setup( x => x.GetEntryAsString( _zip.Object, Meta ) );

			_sut.BuildDocumentInformation( _document );

			MockZipHandler.Verify( x => x.GetEntryAsString( _zip.Object, Meta ) );
		}

		[Test]
		public void Builds_Metadata_Object ()
		{
			MockZipHandler.Setup( x => x.GetEntryAsString( _zip.Object, Meta ) ).Returns( XmlMetadata );
			MockMetadataService.Setup(
				x => x.BuildOdfMetadata( XmlMetadata, MockXmlNamespaceService.Object, MockXDocumentParser.Object ) )
			                   .Returns( new OdfMetadata( null ) );

			var documentInformation = _sut.BuildDocumentInformation( _document );

			MockZipHandler.Verify( x => x.GetEntryAsString( _zip.Object, Meta ) );
			Assert.That( documentInformation.Metadata.Type == null );
		}

		[Test]
		public void Builds_Document_Information ()
		{
			MockZipHandler.Setup( x => x.GetEntryAsString( _zip.Object, Meta ) ).Returns( XmlMetadata );
			MockMetadataService.Setup(
				x => x.BuildOdfMetadata( XmlMetadata, MockXmlNamespaceService.Object, MockXDocumentParser.Object ) )
			                   .Returns( new OdfMetadata( null ) );
			MockZipHandler.Setup( x => x.GetFileType( _zip.Object ) ).Returns( OdfHandlerService.FileType.Ods );

			var documentInformation = _sut.BuildDocumentInformation( _document );

			Mock.Verify( x => x.ZipArchiveFromDocument( _document, ZipArchiveMode.Read ) );

			MockZipHandler.Verify( x => x.GetEntryAsString( _zip.Object, Meta ) );
			Assert.IsNull( documentInformation.Metadata.Type );
			Assert.IsNull( documentInformation.Content );
			Assert.AreEqual( new byte[0], documentInformation.Document );
			Assert.AreEqual( OdfHandlerService.FileType.Ods, documentInformation.FileType );
		}
	}
}