#region Apache License

// Copyright 2014 EventBooking.com, LLC
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

#endregion

#region

using System.IO;
using AntiShaun;
using Moq;
using NUnit.Framework;

#endregion

namespace TestProject
{
	internal class ZipHandlerServiceTests
	{
		private const string TestEntry = "test entry";
		private const string OdsFiletype = "application/vnd.oasis.opendocument.spreadsheet";
		private const string OdtFiletype = "application/vnd.oasis.opendocument.text";
		private static readonly Mock<IStreamReaderWrapperFactory> MockFactory = new Mock<IStreamReaderWrapperFactory>();
		private readonly Mock<IStreamReaderWrapper> _mockStreamReaderWrapper = new Mock<IStreamReaderWrapper>();
		private readonly Mock<IZipArchive> _mockZipArchive = new Mock<IZipArchive>();
		private readonly Mock<IZipEntry> _mockZipEntry = new Mock<IZipEntry>();
		private readonly MemoryStream _stream = new MemoryStream();
		private readonly ZipHandlerService _sut = new ZipHandlerService( MockFactory.Object );

		[Test]
		public void GetsZipEntry ()
		{
			_mockZipArchive.Setup( x => x.GetEntry( TestEntry ) );
			_sut.GetZipEntry( _mockZipArchive.Object, TestEntry );
			_mockZipArchive.Verify( x => x.GetEntry( TestEntry ) );
		}

		[Test]
		public void GetsStringFromEntry ()
		{
			_mockZipArchive.Setup( x => x.GetEntry( TestEntry ) ).Returns( _mockZipEntry.Object );
			_mockZipEntry.Setup( x => x.Open() ).Returns( _stream );
			MockFactory.Setup( x => x.BuildStreamReaderWrapper( _stream ) ).Returns( _mockStreamReaderWrapper.Object );
			_mockStreamReaderWrapper.Setup( x => x.ReadToEnd() ).Returns( OdtFiletype );
			_sut.GetEntryAsString( _mockZipArchive.Object, TestEntry );

			_mockZipArchive.Verify( x => x.GetEntry( TestEntry ) );
			_mockZipEntry.Verify( x => x.Open() );
			MockFactory.Verify( x => x.BuildStreamReaderWrapper( _stream ) );
			_mockStreamReaderWrapper.Verify( x => x.ReadToEnd() );
		}

		[Test]
		public void GetsFileTypeOdt ()
		{
			_mockZipArchive.Setup( x => x.GetEntry( "mimetype" ) ).Returns( _mockZipEntry.Object );
			_mockZipEntry.Setup( x => x.Open() ).Returns( _stream );
			MockFactory.Setup( x => x.BuildStreamReaderWrapper( _stream ) ).Returns( _mockStreamReaderWrapper.Object );
			_mockStreamReaderWrapper.Setup( x => x.ReadToEnd() ).Returns( OdtFiletype );

			var result = _sut.GetFileType( _mockZipArchive.Object );

			Assert.That( result.Equals( OdfHandlerService.FileType.Odt ) );
		}

		[Test]
		public void GetsFileTypeOds ()
		{
			_mockZipArchive.Setup( x => x.GetEntry( "mimetype" ) ).Returns( _mockZipEntry.Object );
			_mockZipEntry.Setup( x => x.Open() ).Returns( _stream );
			MockFactory.Setup( x => x.BuildStreamReaderWrapper( _stream ) ).Returns( _mockStreamReaderWrapper.Object );
			_mockStreamReaderWrapper.Setup( x => x.ReadToEnd() ).Returns( OdsFiletype );

			var result = _sut.GetFileType( _mockZipArchive.Object );

			Assert.That( result.Equals( OdfHandlerService.FileType.Ods ) );
		}
		
		[ExpectedException, Test]
		public void ThrowsExceptionOnIncorrectFileType ()
		{
			_mockZipArchive.Setup( x => x.GetEntry( "mimetype" ) ).Returns( _mockZipEntry.Object );
			_mockZipEntry.Setup( x => x.Open() ).Returns( _stream );
			MockFactory.Setup( x => x.BuildStreamReaderWrapper( _stream ) ).Returns( _mockStreamReaderWrapper.Object );
			_mockStreamReaderWrapper.Setup( x => x.ReadToEnd() ).Returns( "blarg" );
			_sut.GetFileType( _mockZipArchive.Object );
			
		}
	}
}