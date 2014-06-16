using System;
using System.IO;
using RazorEngine;
using System.Xml;
using Encoding = System.Text.Encoding;

namespace DataInjector
{
    public interface IReportTemplateService
    {
        string ApplyTemplate(string templateContent, object model);
    }

    public class ReportTemplateService : IReportTemplateService
    {
        public string ApplyTemplate(string templateContent, object model)
        {
            using (var preparedStream = EncodeAtSigns(templateContent))
            {
                var preparedTemplate = DetectAndConvertTemplateTags(preparedStream);

                return RazorAndReinsertAtSigns(preparedTemplate, model);
            }
        }

        private Stream EncodeAtSigns(String templateContent)
        {
            var preparedContent = templateContent.Replace("@", "U+10FFFD");
            var preparedBytes = Encoding.UTF8.GetBytes(preparedContent);
            return new MemoryStream(preparedBytes);
        }

        private string DetectAndConvertTemplateTags(Stream content)
        {
            //Redo with XDocument? No, it adds a reader in order to handle namespaces, and that's unnecessary complexity.
            //If this code needs to be refactored it may as well be done in xdocument though.

            var n = new NameTable();
            var nsm = new XmlNamespaceManager(n);
            nsm.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
            var document = new XmlDocument();
            document.Load(content);

            XmlNodeList nodes = document.SelectNodes(@"//text:text-input[ @text:description = 'Template']", nsm);

            if (nodes != null)
                foreach (XmlNode xmlNode in nodes)
                {
                    var flagAttr = xmlNode.InnerText;
                    var preparedFlag = flagAttr.Replace("U+10FFFD", "@");
                    var newNode = document.CreateTextNode(preparedFlag);


                    if (xmlNode.ParentNode != null)
                    {
                        xmlNode.ParentNode.ReplaceChild(newNode, xmlNode);
                    }
                    else
                    {
                        Console.WriteLine("Template nodes are in the root of the document! Why is your XML terrible?");
                    }
                }
            return document.OuterXml;
        }

        private string RazorAndReinsertAtSigns(string template, object model)
        {
            var razorOutput = Razor.Parse(template, model);
            return razorOutput.Replace("U+10FFFD", "@");
        }
    }
}