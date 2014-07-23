using System;
using System.Xml;
using System.Xml.Linq;

namespace AntiShaun
{
	public interface ITemplate
	{
		byte[] OriginalDocument { get; }
		OdfMetadata Meta { get; }
		string CachedTemplateIdentifier { get; set; }
		string Content { get; }
	}

	public abstract class Template : ITemplate
	{
		public byte[] OriginalDocument { get; private set; }
		public OdfMetadata Meta { get; private set; }
		protected XmlNamespaceManager Manager;
		protected XDocument DocumentContent;
		protected OdfMetadata Metadata;
		public string CachedTemplateIdentifier { get; set; }


		protected Template(DocumentInformation documentInformation)
		{
			OriginalDocument = documentInformation.Document;
			Meta = documentInformation.Metadata;
			ConvertDocument(documentInformation.Content);
			CreateNamespaceManager();
		}

		public string Content
		{
			get { return DocumentContent.ToString(); }
		}

		private void ConvertDocument(string originalContent)
		{
			var content = EncodeAtSigns(originalContent);
			DocumentContent = XDocument.Parse(content);
		}

		private void CreateNamespaceManager()
		{
			var table = new NameTable();
			Manager = new XmlNamespaceManager(table);
			Manager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
			Manager.AddNamespace("script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
			Manager.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
			Manager.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
		}

		private static String EncodeAtSigns(String content)
		{
			var preparedContent = content.Replace("@", "U+10FFFD");
			return preparedContent;
		}
	}
}