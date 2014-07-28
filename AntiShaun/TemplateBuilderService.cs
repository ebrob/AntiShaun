namespace AntiShaun
{
	public class TemplateBuilderService
	{
		private readonly IOdfHandlerService _odfHandlerService;
		private readonly ITemplateFactory _templateFactory;
		private readonly IXmlNamespaceService _xmlNamespaceService;

		public TemplateBuilderService(ITemplateFactory templateFactory, IOdfHandlerService odfHandlerService, IXmlNamespaceService xmlNamespaceService)
		{
			_templateFactory = templateFactory;
			_odfHandlerService = odfHandlerService;
			_xmlNamespaceService = xmlNamespaceService;
		}

		public Template BuildTemplate(byte[] document)
		{
			var documentInformation = _odfHandlerService.BuildDocumentInformation(document);
			var template = _templateFactory.GenerateTemplate(documentInformation, _xmlNamespaceService);
			return template;
		}
	}
}