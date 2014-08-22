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
using System.IO.Compression;
using AntiShaun;
using Moq;
using NUnit.Framework;
using RazorEngine.Templating;
using ITemplate = AntiShaun.ITemplate;

#endregion

namespace TestProject
{
	internal class ReportGeneratorServiceTests
	{
		private static readonly Mock<IZipFactory> MockZipFactory = new Mock<IZipFactory>();
		private readonly Mock<ITemplate> _mockTemplate = new Mock<ITemplate>();
		private readonly Mock<ITemplateService> _mockTemplateService = new Mock<ITemplateService>();
		private readonly Mock<IZipArchive> _mockZipArchive = new Mock<IZipArchive>();
		private readonly Mock<IZipEntry> _mockZipEntry = new Mock<IZipEntry>();
		private readonly ReportGeneratorService _sut = new ReportGeneratorService( MockZipFactory.Object );
		private readonly Stream _testStream = new MemoryStream();


		[Test]
		public void BuildsReport ()
		{
			MockZipFactory.Setup( x => x.ZipArchiveFromStream( It.IsAny<Stream>(), ZipArchiveMode.Update ) )
			              .Returns( _mockZipArchive.Object );

			_mockTemplateService.Setup( x => x.Run( It.IsAny<string>(), It.IsAny<object>(), null ) )
			                    .Returns( "hello, this is a test string" );

			_mockZipArchive.Setup( x => x.GetEntry( "content.xml" ) ).Returns( _mockZipEntry.Object );

			_sut.BuildReport( _mockTemplate.Object, typeof( TestModel ), _testStream, _mockTemplateService.Object );
		}
	}
}
	;