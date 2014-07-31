#region

using System;
using System.Xml;
using System.Xml.XPath;

#endregion

namespace AntiShaun
{
	public interface IBuildOdfMetadataService
	{
		OdfMetadata BuildOdfMetadata(string metaXml, IXmlNamespaceResolver manager, IXDocumentParserService parserService);
	}

	public class BuildOdfMetadataService : IBuildOdfMetadataService
	{
		public OdfMetadata BuildOdfMetadata(string metaXml, IXmlNamespaceResolver manager, IXDocumentParserService parserService)
		{
			var document = parserService.Parse(metaXml);
			var modelTypeNameElement =
				document.XPathSelectElement(@"//meta:user-defined[ @meta:name = 'ModelType' ]", manager);

			var modelTypeName = modelTypeNameElement.Value;

			var type = Type.GetType(modelTypeName); 
			return new OdfMetadata(type);

		}
	}
}