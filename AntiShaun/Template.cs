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
		OdfMetadata Meta { get; }
		string CachedTemplateIdentifier { get; set; }
		string Content { get; }
	}

	public abstract class Template : ITemplate
	{
		protected XDocument DocumentContent;
		protected IXmlNamespaceResolver Manager;
		protected OdfMetadata Metadata;


		protected Template(DocumentInformation documentInformation, IXmlNamespaceService xmlNamespaceService)
		{
			OriginalDocument = documentInformation.Document;
			Meta = documentInformation.Metadata;
			ConvertDocument(documentInformation.Content);
			Manager = xmlNamespaceService;
		}

		public byte[] OriginalDocument { get; private set; }
		public OdfMetadata Meta { get; private set; }
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



		private static String EncodeAtSigns(String content)
		{
			var preparedContent = content.Replace("@", "U+10FFFD");
			return preparedContent;
		}
	}
}