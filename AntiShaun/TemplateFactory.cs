using System;

namespace AntiShaun
{
	public interface ITemplateFactory
	{
		Template GenerateTemplate(DocumentInformation documentInformation);
	}

	public class TemplateFactory : ITemplateFactory
	{
		public Template GenerateTemplate(DocumentInformation documentInformation)
		{
			switch (documentInformation.FileType)
			{
				case OdfHandlerService.FileType.Ods:
					return new OdsTemplate(documentInformation);

				case OdfHandlerService.FileType.Odt:
					return new OdtTemplate(documentInformation);
			}

			throw new NotSupportedException(
				"Only ods and odt files are supported at this time, and this error should have been caught on pattern initialization.");
		}
	}
}