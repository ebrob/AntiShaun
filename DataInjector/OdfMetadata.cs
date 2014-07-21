using System;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DataInjector
{
    public class OdfMetadata
    {
        private readonly Type _type;

        public Type Type
        {
            get { return _type; }
        }


        public OdfMetadata(string metaXml)
        {
            var document = XDocument.Load(metaXml);
            var modelTypeNameElement =
                document.XPathSelectElement(@"//meta:user-defined[ @meta:name = 'ModelType' ]/.");

            var modelTypeName = modelTypeNameElement.Value;

            _type = Type.GetType(modelTypeName);

        }
    }
}