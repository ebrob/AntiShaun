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

using System.Xml;

#endregion

namespace AntiShaun
{
	public class TemplateBuilderService
	{
		private readonly IOdfHandlerService _odfHandlerService;
		private readonly ITemplateFactory _templateFactory;
		private readonly IXDocumentParserService _xDocumentParserService;
		private readonly IXmlNamespaceResolver _xmlNamespaceService;

		public TemplateBuilderService( ITemplateFactory templateFactory, IOdfHandlerService odfHandlerService,
		                               IXmlNamespaceResolver xmlNamespaceService,
		                               IXDocumentParserService xDocumentParserService )
		{
			_templateFactory = templateFactory;
			_odfHandlerService = odfHandlerService;
			_xmlNamespaceService = xmlNamespaceService;
			_xDocumentParserService = xDocumentParserService;
		}

		public Template BuildTemplate( byte[] document )
		{
			var documentInformation = _odfHandlerService.BuildDocumentInformation( document );
			var template = _templateFactory.GenerateTemplate( documentInformation, _xmlNamespaceService, _xDocumentParserService );
			return template;
		}
	}
}