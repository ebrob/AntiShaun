using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath; 


namespace DataInjector
{
    internal class OdtTemplate : Template
    {
        public OdtTemplate(DocumentInformation documentInformation)
            : base(documentInformation)
        {
            DocumentContent = ScriptSectionFormatting(DocumentContent, Manager);
            DocumentContent = ReplaceFields(DocumentContent, Manager);
        }

        private static XDocument ScriptSectionFormatting(XDocument document, XmlNamespaceManager manager)
        {
            var targetScripts =
                document.XPathSelectElements(@"//text:script[@script:language = 'Template']", manager).ToList();

            foreach (
                var script in
                    targetScripts.Where(script => script.Value.Contains("foreach") || script.Value.Contains("if")))
            {
                CreateControlFlowSection(script, manager);
            }
            return document;
        }

        private static void CreateControlFlowSection(XElement script, XmlNamespaceManager manager)
        {
//TODO: Test this method

            var parentSection = script.XPathSelectElement("./ancestor::text:section", manager);
            // TODO: If ParentSection is null, throw specific exception

            var scriptValue = script.Value.Replace("U+10FFFD", "@");

            var beforeNode = new XText(scriptValue + "{");

            var afterNode = new XText("}");

            parentSection.AddBeforeSelf(beforeNode);

            parentSection.AddAfterSelf(afterNode);

            script.Remove();
        }

        private static XDocument ReplaceFields(XDocument document, XmlNamespaceManager manager)
        {
            var elements = document.XPathSelectElements(@"//text:text-input[ @text:description = 'Template']",
                                                        manager);
            var nodes = elements.ToList();
            foreach (var element in nodes)
            {
                var attribute = element.Value;
                var preparedAttribute = attribute.Replace("U+10FFFD", "@");
                var text = new XText(preparedAttribute);
                element.ReplaceWith(text);
            }
            return document;
        }
    }
}