/*Copyright 2014 EventBooking.com, LLC

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. 
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, 
software distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and limitations under the License
*/
#region

using System;
using System.Xml;
using System.Xml.Linq;

#endregion

namespace AntiShaun
{
	public interface ITemplate
	{
		OdfMetadata Meta { get; }
		string CachedTemplateIdentifier { get; set; }
		string Content { get; }
	}

	public abstract class Template : ITemplate
	{
		protected XDocument DocumentContent;
		protected readonly IXmlNamespaceResolver Manager;

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