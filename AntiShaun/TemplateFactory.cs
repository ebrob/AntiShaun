#region

using System;

#endregion

namespace AntiShaun
{
	public interface ITemplateFactory
	{
		Template GenerateTemplate(DocumentInformation documentInformation, IXmlNamespaceService xmlNamespaceService);
	}

	public class TemplateFactory : ITemplateFactory
	{
		public Template GenerateTemplate(DocumentInformation documentInformation, IXmlNamespaceService xmlNamespaceService)
		{
			switch (documentInformation.FileType)
			{
				case OdfHandlerService.FileType.Ods:
					return new OdsTemplate(documentInformation, xmlNamespaceService);

				case OdfHandlerService.FileType.Odt:
					return new OdtTemplate(documentInformation, xmlNamespaceService);
			}

			throw new NotSupportedException(
				"Only ods and odt files are supported at this time, and this error should have been caught on pattern initialization.");
		}
	}
}