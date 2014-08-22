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

using System;
using System.Xml;

#endregion

namespace AntiShaun
{
	public interface ITemplateFactory
	{
		Template GenerateTemplate( DocumentInformation documentInformation, IXmlNamespaceResolver xmlNamespaceService,
		                           IXDocumentParserService xDocumentParserService );
	}

	public class TemplateFactory : ITemplateFactory
	{
		public Template GenerateTemplate( DocumentInformation documentInformation, IXmlNamespaceResolver xmlNamespaceService,
		                                  IXDocumentParserService xDocumentParserService )
		{
			switch( documentInformation.FileType )
			{
				case OdfHandlerService.FileType.Ods:
					return new OdsTemplate( documentInformation, xmlNamespaceService, xDocumentParserService );

				case OdfHandlerService.FileType.Odt:
					return new OdtTemplate( documentInformation, xmlNamespaceService, xDocumentParserService );
			}

			throw new NotSupportedException(
				"Only ods and odt files are supported at this time, and this error should have been caught on pattern initialization." );
		}
	}
}