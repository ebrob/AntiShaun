using System;

namespace DataInjector
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

        public Template BuildTemplate(byte[] document, Type modelType)
        {
            var documentInformation = _odfHandlerService.BuildDocumentInformation(document, modelType);
            var template = _templateFactory.GenerateTemplate(documentInformation);
            return template;
        }
    }
}