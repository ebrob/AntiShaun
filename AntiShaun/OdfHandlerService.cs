#region

using System.Xml;

#endregion

namespace AntiShaun
{
	public interface IOdfHandlerService
	{
		DocumentInformation BuildDocumentInformation(byte[] document);
	}

	public class OdfHandlerService : IOdfHandlerService //TODO: Separate concerns?
	{
		public enum FileType
		{
			Unknown,
			Odt,
			Ods
		}

		private readonly IFileHandlerService _fileHandlerService;
		private readonly XmlNamespaceManager _manager;
		private readonly IZipHandlerService _zipHandlerService;

		public OdfHandlerService(IFileHandlerService fileHandlerService, IZipHandlerService zipHandlerService)
		{
			_fileHandlerService = fileHandlerService;
			_zipHandlerService = zipHandlerService;

			var table = new NameTable();
			_manager = new XmlNamespaceManager(table);
			_manager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
			_manager.AddNamespace("script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
			_manager.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
			_manager.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
			_manager.AddNamespace("meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
		}

		public virtual DocumentInformation BuildDocumentInformation(byte[] document)
		{
			using (var archive = _fileHandlerService.ZipArchiveFromDocument(document))
			{
				var fileType = _zipHandlerService.GetFileType(archive);
				var content = _zipHandlerService.GetEntryAsString(archive, "content.xml");
				var metaXml = _zipHandlerService.GetEntryAsString(archive, "meta.xml");

				var metadata = new OdfMetadata(metaXml, _manager);

				var information = new DocumentInformation(fileType, document, content, metadata);

				return information;
			}
		}
	}
}