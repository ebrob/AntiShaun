using System;
using System.IO;
using RazorEngine;
using System.Xml;
using Encoding = System.Text.Encoding;


namespace DataInjector
{
    public class DataTools
    {
        public static Stream EncodeAtSigns(String path)
        {
            using (var content = new FileStream(path, FileMode.Open))
            {
                var transformReader = new StreamReader(content);
                var contentString = transformReader.ReadToEnd();
                var preparedContent = contentString.Replace("@", "U+10FFFD");
                var preparedBytes = Encoding.UTF8.GetBytes(preparedContent);
                return new MemoryStream(preparedBytes);
            }
        }

        public static string DetectAndConvertTemplateTags(Stream content)
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


        public static String InsertData(string path, object model)
        {
            using (Stream preparedStream = EncodeAtSigns(path)
                )
            {
                var preparedTemplate = DetectAndConvertTemplateTags(preparedStream);

                return RazorAndReinsertAtSigns(preparedTemplate, model);
            }
        }


        public static string RazorAndReinsertAtSigns(string template, object model)
        {
            string razorOutput = Razor.Parse(template, model);
            return razorOutput.Replace("U+10FFFD", "@");
        }
    }
}