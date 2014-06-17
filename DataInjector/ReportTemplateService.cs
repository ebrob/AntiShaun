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

                return RazorParseAndReinsertAtSigns(preparedTemplate, model);
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
//TODO: refactor for same level of abstraction
            XDocument document = XDocument.Load(content);

            var table = new NameTable();
            var manager = new XmlNamespaceManager(table);
            manager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");

            document = SectionFormatting(document, manager);
            document = ReplaceFields(document, manager);


            return document.ToString();
        }

        private XDocument SectionFormatting(XDocument document, XmlNamespaceManager manager)
        {
            manager.AddNamespace("script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
            var targetScripts =
                document.XPathSelectElements(@"//text:script[@script:language = 'Template']", manager).ToList();

            foreach (var script in targetScripts)
            {
                if (script.Value.Contains("foreach"))
                {
                }
                else if (script.Value.Contains("if"))
                {
                }
            }
            return document;
        }


        private XDocument ReplaceFields(XDocument document, XmlNamespaceManager manager)
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


        private XDocument InsertLoops(XElement script, XmlNamespaceManager manager)
        {
//TODO: Test this method

            //TODO: fix the thing

        
            var parentSection = script.XPathSelectElement("./ancestor::text:section", manager);
            
            var scriptValue = script.Value.Replace("U+10FFFD", "@");
            
            var beforeNode = new XText(scriptValue + "{");
            
            var afterNode = new XText("}");
            
            parentSection.AddBeforeSelf(beforeNode);
        
            parentSection.AddAfterSelf(afterNode);
        
            script.Remove();
            return script.Document;
        }


        private
            string RazorParseAndReinsertAtSigns(string template, object model)
        {
            var razorOutput = Razor.Parse(template, model);
            return razorOutput.Replace
                ("U+10FFFD", "@");
        }
    }
}