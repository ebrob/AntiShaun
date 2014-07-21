using System;

namespace DataInjector
{
    internal class TemplateBuilderService
    {
        private readonly ITemplateFactory _templateFactory;
        private readonly IDataHandlerService _dataHandlerService;

        public TemplateBuilderService(ITemplateFactory templateFactory, IDataHandlerService dataHandlerService)
        {
            _templateFactory = templateFactory;
            _dataHandlerService = dataHandlerService;
        }

        public Template BuildTemplate(byte[] document, Type modelType)
        {
            var documentInformation = _dataHandlerService.BuildDocumentInformation(document, modelType);
            var template = _templateFactory.GenerateTemplate(documentInformation);
            return template;
        }
    }
}