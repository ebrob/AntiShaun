namespace AntiShaun
{
	public class TemplateBuilderService
	{
		private readonly ITemplateFactory _templateFactory;
		private readonly IOdfHandlerService _odfHandlerService;

		public TemplateBuilderService(ITemplateFactory templateFactory, IOdfHandlerService odfHandlerService)
		{
			_templateFactory = templateFactory;
			_odfHandlerService = odfHandlerService;
		}

		public Template BuildTemplate(byte[] document)
		{
			var documentInformation = _odfHandlerService.BuildDocumentInformation(document);
			var template = _templateFactory.GenerateTemplate(documentInformation);
			return template;
		}
	}
}