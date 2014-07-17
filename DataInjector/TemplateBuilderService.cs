using System;

namespace DataInjector
{
    class TemplateBuilderService
    {
        public Template BuildTemplate(byte[] document, Type modelType, IDataHandlerService dataHandlerService)
        {
            var documentInformation = dataHandlerService.BuildDocumentInformation(document, modelType);
            var factory = new TemplateFactory();
            var template = factory.GenerateTemplate(documentInformation);
            return template;
        }
    }
}
