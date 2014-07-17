using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using EnsureThat;

namespace DataInjector
{
    internal class OdsTemplate : Template
    {
        public OdsTemplate(DocumentInformation documentInformation)
            : base(documentInformation)
        {
            DocumentContent = HandleConditionals(DocumentContent, Manager);
            DocumentContent = ReplaceComments(DocumentContent, Manager);
        }

        private static XDocument HandleConditionals(XDocument document, XmlNamespaceManager manager)
        {
            var targetComments =
                document.XPathSelectElements(@"//text:span", manager).ToList();

            foreach (
                var comment in
                    targetComments.Where(script => script.Value.Contains("foreach") || script.Value.Contains("if")))
            {
                CreateControlFlowFromComment(comment, manager);
            }
            return document;
        }

        private static void CreateControlFlowFromComment(XElement comment, XmlNamespaceManager manager)
        {
            var row = comment.XPathSelectElement("./ancestor::table:table-row", manager);
            var commentValue = comment.Value.Replace("U+10FFFD", "@");

            var beforeNode = new XText(commentValue + "{");

            var afterNode = new XText("}");

            row.AddBeforeSelf(beforeNode);
            row.AddAfterSelf(afterNode);
            comment.Remove();
        }


        private static XDocument ReplaceComments(XDocument document, XmlNamespaceManager manager)
        {
            var targetCells = document.XPathSelectElements(@"//office:annotation/..", manager).ToList();
            foreach (var cell in targetCells)
            {
                var comment = cell.XPathSelectElement(@"./office:annotation/text:p", manager);
                var preparedValue = comment.Value.Replace("U+10FFFD", "@");
                var parent = comment.Parent;
                Ensure.That(parent).IsNotNull();
                if (parent != null) parent.Remove();

                XNamespace textNs = "urn:oasis:names:tc:opendocument:xmlns:text:1.0";
                var content = new XElement(textNs + "p") {Value = preparedValue};
                cell.Add(content);
            }
            return document;
        }
    }
}