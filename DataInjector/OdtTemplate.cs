using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;


namespace AntiShaun
{
    internal class OdtTemplate : Template
    {
        public OdtTemplate(DocumentInformation documentInformation)
            : base(documentInformation)
        {
            ScriptSectionFormatting();
            ReplaceFields();
        }

        private void ScriptSectionFormatting()
        {
            var targetScripts =
                DocumentContent.XPathSelectElements(@"//text:script[@script:language = 'Template']", Manager).ToList();

            foreach (
                var script in
                    targetScripts.Where(script => script.Value.Contains("foreach") || script.Value.Contains("if")))
            {
                CreateControlFlowSection(script);
            }
        }

        private void CreateControlFlowSection(XElement script)
        {
//TODO: Test this method

            var parentSection = script.XPathSelectElement("./ancestor::text:section", Manager);
            // TODO: If ParentSection is null, throw specific exception

            var scriptValue = script.Value.Replace("U+10FFFD", "@");

            var beforeNode = new XText(scriptValue + "{");

            var afterNode = new XText("}");

            parentSection.AddBeforeSelf(beforeNode);

            parentSection.AddAfterSelf(afterNode);

            script.Remove();
        }

        private void ReplaceFields()
        {
            var elements = DocumentContent.XPathSelectElements(@"//text:text-input[ @text:description = 'Template']",
                                                               Manager);
            var nodes = elements.ToList();
            foreach (var element in nodes)
            {
                var attribute = element.Value;
                var preparedAttribute = attribute.Replace("U+10FFFD", "@");
                var text = new XText(preparedAttribute);
                element.ReplaceWith(text);
            }
        }
    }
}