using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using EnsureThat;

namespace AntiShaun
{
    internal class OdsTemplate : Template
    {


        public OdsTemplate(DocumentInformation documentInformation)
            : base(documentInformation)
        {
            HandleConditionals();
            ReplaceComments();
        }

        private void HandleConditionals()
        {
            var targetComments =
                DocumentContent.XPathSelectElements(@"//text:span", Manager).ToList();

            foreach (
                var comment in
                    targetComments.Where(script => script.Value.Contains("foreach") || script.Value.Contains("if")))
            {
                CreateControlFlowFromComment(comment);
            }
        }

        private void CreateControlFlowFromComment(XElement comment)
        {
            var row = comment.XPathSelectElement("./ancestor::table:table-row", Manager);
            var commentValue = comment.Value.Replace("U+10FFFD", "@");

            var beforeNode = new XText(commentValue + "{");

            var afterNode = new XText("}");

            row.AddBeforeSelf(beforeNode);
            row.AddAfterSelf(afterNode);
            comment.Remove();
        }


        private void ReplaceComments()
        {
            var targetCells = DocumentContent.XPathSelectElements(@"//office:annotation/..", Manager).ToList();
            foreach (var cell in targetCells)
            {
                var comment = cell.XPathSelectElement(@"./office:annotation/text:p", Manager);
                var preparedValue = comment.Value.Replace("U+10FFFD", "@");
                var parent = comment.Parent;
                Ensure.That(parent).IsNotNull();
                if (parent != null) parent.Remove();

                XNamespace textNs = "urn:oasis:names:tc:opendocument:xmlns:text:1.0";
                var content = new XElement(textNs + "p") {Value = preparedValue};
                cell.Add(content);
            }
        }
    }
}