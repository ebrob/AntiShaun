using System;

namespace DataInjector
{
    public class TemplateFactory
    {
        public Template GenerateTemplate(DocumentInformation documentInformation)
        {
            switch (documentInformation.FileType)
            {
                case DataHandlerService.FileType.Ods:
                    return new OdsTemplate(documentInformation);

                case DataHandlerService.FileType.Odt:
                    return new OdtTemplate(documentInformation);
            }

            throw new NotSupportedException("Only ods and odt files are supported at this time, and this error should have been caught on pattern initialization.");
        }
    }
}