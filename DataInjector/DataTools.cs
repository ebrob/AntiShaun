using System;
using System.Globalization;
using System.IO;
using System.Text;
using RazorEngine;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Linq;
using Encoding = System.Text.Encoding;


namespace DataInjector
{
    internal class DataTools
    {
        public static String InsertDate(Stream content)
        {
            var transformReader = new StreamReader(content);
            var contentString = transformReader.ReadToEnd();
            var preparedContent = contentString.Replace("@", "BLURGTWONK");
            var preparedBytes = Encoding.UTF8.GetBytes(preparedContent);
            var preparedStream = new MemoryStream(preparedBytes);

            //TODO: Redo with XDocument?

            var n = new NameTable();
            var nsm = new XmlNamespaceManager(n);
            nsm.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
            var document = new XmlDocument();
            document.Load(preparedStream);

            XmlNodeList nodes = document.SelectNodes(@"//text:text-input[ @text:description = 'Template']", nsm);

            if (nodes != null)
                foreach (XmlNode xmlNode in nodes)
                {
                    var flagAttr = xmlNode.InnerText; // .Attributes["text:description"]; //, "urn:oasis:names:tc:opendocument:xmlns:text:1.0"];
                    var preparedFlag = flagAttr.Replace("BLURGTWONK", "@");
                    //If our node has no parent then something is wrong enough to warrant throwing an exception.
                    var newNode = document.CreateTextNode(preparedFlag);
                    //xmlNode.ParentNode.AppendChild(newNode);
                    xmlNode.ParentNode.ReplaceChild(newNode, xmlNode);
                }
            var model = new {Date = DateTime.Today, Name = "Fancypants McSnooterson"};
            string razorOutput = Razor.Parse(document.OuterXml, model);
            return razorOutput.Replace("BLURGTWONK", "@");
        }
    }
}