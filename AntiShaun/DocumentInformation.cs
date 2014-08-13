#region

using System;

#endregion

namespace AntiShaun
{
	public class DocumentInformation
	{
		public DocumentInformation(OdfHandlerService.FileType fileType, Byte[] document, string content, OdfMetadata metadata)
		{
			FileType = fileType;
			Document = document;
			Content = content;
			Metadata = metadata;
		}

		public OdfHandlerService.FileType FileType { get; private set; }
		public Byte[] Document { get; private set; }
		public String Content { get; private set; }
		public OdfMetadata Metadata { get; private set; }
	}
}