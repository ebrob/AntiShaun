#region

using System;
using System.Xml;
using System.Xml.Linq;

#endregion

namespace AntiShaun
{
	public interface ITemplate
	{
		byte[] OriginalDocument { get; }
		IOdfMetadata Meta { get; }
		string CachedTemplateIdentifier { get; set; }
		string Content { get; }
	}

	public abstract class Template : ITemplate
	{
		protected XDocument DocumentContent;
		protected XmlNamespaceManager Manager;
		protected OdfMetadata Metadata;


		protected Template(DocumentInformation documentInformation)
		{
			OriginalDocument = documentInformation.Document;
			Meta = documentInformation.Metadata;
			ConvertDocument(documentInformation.Content);
			CreateNamespaceManager();
		}

		public byte[] OriginalDocument { get; private set; }
		public IOdfMetadata Meta { get; private set; }
		public string CachedTemplateIdentifier { get; set; }

		public virtual string Content
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