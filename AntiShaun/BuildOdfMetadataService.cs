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

#region Using directives

using System;
using System.Xml;
using System.Xml.XPath;

#endregion

namespace AntiShaun
{
	public interface IBuildOdfMetadataService
	{
		OdfMetadata BuildOdfMetadata( string metaXml, IXmlNamespaceResolver manager, IXDocumentParserService parserService );
	}

	public class BuildOdfMetadataService : IBuildOdfMetadataService
	{
		public OdfMetadata BuildOdfMetadata( string metaXml, IXmlNamespaceResolver manager,
		                                     IXDocumentParserService parserService )
		{
			var document = parserService.Parse( metaXml );
			var modelTypeNameElement =
				document.XPathSelectElement( @"//meta:user-defined[ @meta:name = 'ModelType' ]", manager );

			var modelTypeName = modelTypeNameElement.Value;

			var type = Type.GetType( modelTypeName, true );
			return new OdfMetadata( type );
		}
	}
}