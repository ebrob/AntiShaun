#region

using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

#endregion

namespace AntiShaun
{
	public interface IBuildOdfMetadataService
	{
		OdfMetadata BuildOdfMetadata(string metaXml, IXmlNamespaceResolver manager);
	}

	public class BuildOdfMetadataService : IBuildOdfMetadataService
	{
		public OdfMetadata BuildOdfMetadata(string metaXml, IXmlNamespaceResolver manager)
		{
			var document = XDocument.Parse(metaXml);
			var modelTypeNameElement =
				document.XPathSelectElement(@"//meta:user-defined[ @meta:name = 'ModelType' ]", manager);

			var modelTypeName = modelTypeNameElement.Value;

			var type = Type.GetType(modelTypeName);
			return new OdfMetadata(type);

		}
	}
}