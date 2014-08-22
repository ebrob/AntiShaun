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
using System.Reflection;
using System.Xml.Linq;
using AntiShaun;
using Moq;
using NUnit.Framework;

#endregion

namespace TestProject
{
	public class TemplateFactoryTests
	{
		private static string _content;


		private readonly TemplateFactory _sut = new TemplateFactory();

		private readonly Mock<IXDocumentParserService> _xDocumentParserService = new Mock<IXDocumentParserService>();
		private readonly XmlNamespaceService _xmlNamespaceService = new XmlNamespaceService();
		private DocumentInformation _odtDocumentInformation;

		[Test]
		public void GeneratesOdtTemplate ()
		{
			var asm = Assembly.GetExecutingAssembly();
			using( var stream = asm.GetManifestResourceStream( "TestProject.OdtContent.xml" ) )
			{
				if( stream == null ) return;
				var reader = new StreamReader( stream );
				_content = reader.ReadToEnd();
			}


			_odtDocumentInformation = new DocumentInformation(
				OdfHandlerService.FileType.Odt,
				new byte[0], _content,
				new OdfMetadata( typeof( object ) ) );


			_xDocumentParserService.Setup( x => x.Parse( It.IsAny<string>() ) ).Returns( XDocument.Parse( _content ) );


			_sut.GenerateTemplate( _odtDocumentInformation, _xmlNamespaceService, _xDocumentParserService.Object );


			_xDocumentParserService.Verify( x => x.Parse( It.IsAny<string>() ) );
		}

		[Test]
		public void GeneratesOdsTemplate ()
		{
			var asm = Assembly.GetExecutingAssembly();
			using( var stream = asm.GetManifestResourceStream( "TestProject.OdsContent.xml" ) )
			{
				if( stream == null ) return;
				var reader = new StreamReader( stream );
				_content = reader.ReadToEnd();
			}


			_odtDocumentInformation = new DocumentInformation(
				OdfHandlerService.FileType.Ods,
				new byte[0], _content,
				new OdfMetadata( typeof( object ) ) );


			_xDocumentParserService.Setup( x => x.Parse( It.IsAny<string>() ) ).Returns( XDocument.Parse( _content ) );


			_sut.GenerateTemplate( _odtDocumentInformation, _xmlNamespaceService, _xDocumentParserService.Object );


			_xDocumentParserService.Verify( x => x.Parse( It.IsAny<string>() ) );
		}
	}
}