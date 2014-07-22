using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AntiShaun
{
    public class OdfMetadata
    {
        private readonly Type _type;

        public Type Type
        {
            get { return _type; }
        }


        public OdfMetadata(string metaXml, XmlNamespaceManager manager)
        {
            var document = XDocument.Parse(metaXml);
             var modelTypeNameElement =
                document.XPathSelectElement(@"//meta:user-defined[ @meta:name = 'ModelType' ]", manager);

            var modelTypeName = modelTypeNameElement.Value;

            _type = Type.GetType(modelTypeName);

        }
    }
}