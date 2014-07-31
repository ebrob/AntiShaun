namespace AntiShaun
{
	public interface IOdfHandlerService
	{
		DocumentInformation BuildDocumentInformation(byte[] document);
	}

	public class OdfHandlerService : IOdfHandlerService
	{
		public enum FileType
		{
			Odt,
			Ods
		}

		private readonly IBuildOdfMetadataService _buildOdfMetadataService;

		private readonly IFileHandlerService _fileHandlerService;
		private readonly IXmlNamespaceService _manager;
		private readonly IZipHandlerService _zipHandlerService;
		private readonly IXDocumentParserService _ixDocumentParserService;

		public OdfHandlerService(IFileHandlerService fileHandlerService, IZipHandlerService zipHandlerService,
		                         IBuildOdfMetadataService buildOdfMetadataService, IXmlNamespaceService xmlNamespaceService, IXDocumentParserService ixDocumentParserService)
		{
			_fileHandlerService = fileHandlerService;
			_zipHandlerService = zipHandlerService;
			_buildOdfMetadataService = buildOdfMetadataService;
			_manager = xmlNamespaceService;
			_ixDocumentParserService = ixDocumentParserService;
		}

		public virtual DocumentInformation BuildDocumentInformation(byte[] document)
		{
			using (var archive = _fileHandlerService.ZipArchiveFromDocument(document))
			{
				var fileType = _zipHandlerService.GetFileType(archive);
				var content = _zipHandlerService.GetEntryAsString(archive, "content.xml");
				var metaXml = _zipHandlerService.GetEntryAsString(archive, "meta.xml");

				var metadata = _buildOdfMetadataService.BuildOdfMetadata(metaXml, _manager, _ixDocumentParserService);

				var information = new DocumentInformation(fileType, document, content, metadata);

				return information;
			}
		}
	}
}