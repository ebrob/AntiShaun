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

#region Using directives

using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using AntiShaun;
using Moq;
using NUnit.Framework;

#endregion

namespace TestProject
{
	public class BuildOdfMetadataServiceTests
	{
		private readonly XmlNamespaceManager _namespaces = new XmlNamespaceManager( new NameTable() );
		private readonly Mock<IXDocumentParserService> _parserMock = new Mock<IXDocumentParserService>();
		private readonly BuildOdfMetadataService _sut = new BuildOdfMetadataService();
		private string _metaXml;

		[SetUp]
		public void Setup ()
		{
			_namespaces.AddNamespace( "meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0" );

			var asm = Assembly.GetExecutingAssembly();
			using( var stream = asm.GetManifestResourceStream( "TestProject.Meta.xml" ) )
			{
				if( stream == null ) return;
				var reader = new StreamReader( stream );
				_metaXml = reader.ReadToEnd();
			}
			_parserMock.Setup( x => x.Parse( _metaXml ) ).Returns( XDocument.Parse( _metaXml ) );
		}

		[Test]
		public void ParsesMetaXml ()
		{
			_sut.BuildOdfMetadata( _metaXml, _namespaces, _parserMock.Object );
			_parserMock.Verify( x => x.Parse( _metaXml ) );
		}
	}
}