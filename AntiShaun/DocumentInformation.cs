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

		public OdfHandlerService.FileType FileType { get; set; }
		public Byte[] Document { get; set; }
		public String Content { get; set; }
		public OdfMetadata Metadata { get; set; }
	}
}