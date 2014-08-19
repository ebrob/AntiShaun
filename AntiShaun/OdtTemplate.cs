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

using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

#endregion

namespace AntiShaun
{
	internal class OdtTemplate : Template
	{
		public OdtTemplate( DocumentInformation documentInformation, IXmlNamespaceResolver xmlNamespaceResolver,
		                    IXDocumentParserService xDocumentParserService )
			: base( documentInformation, xmlNamespaceResolver, xDocumentParserService )
		{
			ScriptSectionFormatting();
			ReplaceFields();
		}

		private void ScriptSectionFormatting ()
		{
			var targetScripts =
				DocumentContent.XPathSelectElements( @"//text:script[@script:language = 'Template']", Manager ).ToList();

			foreach(var script in targetScripts.Where( script => script.Value.Contains( "foreach" ) || script.Value.Contains( "if" ) ) )
			{
				CreateControlFlowSection( script );
			}
		}

		private void CreateControlFlowSection( XElement script )
		{
//TODO: Test this method

			var parentSection = script.XPathSelectElement( "./ancestor::text:section", Manager );
			// TODO: If ParentSection is null, throw specific exception

			var scriptValue = script.Value.Replace( "U+10FFFD", "@" );

			var beforeNode = new XText( scriptValue + "{" );

			var afterNode = new XText( "}" );

			parentSection.AddBeforeSelf( beforeNode );

			parentSection.AddAfterSelf( afterNode );

			script.Remove();
		}

		private void ReplaceFields ()
		{
			var elements = DocumentContent.XPathSelectElements( @"//text:text-input[ @text:description = 'Template']",
			                                                    Manager );
			var nodes = elements.ToList();
			foreach( var element in nodes )
			{
				var attribute = element.Value;
				var preparedAttribute = attribute.Replace( "U+10FFFD", "@" );
				var text = new XText( preparedAttribute );
				element.ReplaceWith( text );
			}
		}
	}
}