using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using RazorEngine;
using System.Xml;
using System.Xml.Linq;
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
        
            XDocument document = XDocument.Load(content);

            var table = new NameTable();
            var manager = new XmlNamespaceManager(table);
            manager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
            
            var elements = document.XPathSelectElements(@"//text:text-input[ @text:description = 'Template']", manager);
            var nodes = elements.ToList();
            foreach (var element in nodes)
            {
                var attribute = element.Value;
                var preparedAttribute = attribute.Replace("U+10FFFD", "@");
                var text = new XText(preparedAttribute);
                element.ReplaceWith(text);
            }
            return document.ToString();

          
        }

        private string RazorAndReinsertAtSigns(string template, object model)
        {
            var razorOutput = Razor.Parse(template, model);
            return razorOutput.Replace("U+10FFFD", "@");
        }
    }
}