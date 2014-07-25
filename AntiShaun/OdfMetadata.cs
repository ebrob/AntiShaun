#region
using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

#endregion

namespace AntiShaun
{
	public interface IOdfMetadata
	{
		Type Type { get; }
	}

	public class OdfMetadata : IOdfMetadata
	{
		private readonly Type _type;


		public OdfMetadata(string metaXml, XmlNamespaceManager manager)
		{
			var document = XDocument.Parse(metaXml);
			var modelTypeNameElement =
				document.XPathSelectElement(@"//meta:user-defined[ @meta:name = 'ModelType' ]", manager);

			var modelTypeName = modelTypeNameElement.Value;

			_type = Type.GetType(modelTypeName);
		}

		public virtual Type Type
		{
			get { return _type; }
		}
	}
}