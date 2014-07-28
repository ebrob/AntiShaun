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

		private readonly IBuildOdfMetadataService _buildOdfMetadataService;

		private readonly IFileHandlerService _fileHandlerService;
		private readonly IXmlNamespaceService _manager;
		private readonly IZipHandlerService _zipHandlerService;

		public OdfHandlerService(IFileHandlerService fileHandlerService, IZipHandlerService zipHandlerService,
		                         IBuildOdfMetadataService buildOdfMetadataService, IXmlNamespaceService xmlNamespaceService)
		{
			_fileHandlerService = fileHandlerService;
			_zipHandlerService = zipHandlerService;
			_buildOdfMetadataService = buildOdfMetadataService;
			_manager = xmlNamespaceService;
		}

		public virtual DocumentInformation BuildDocumentInformation(byte[] document)
		{
			using (var archive = _fileHandlerService.ZipArchiveFromDocument(document))
			{
				var fileType = _zipHandlerService.GetFileType(archive);
				var content = _zipHandlerService.GetEntryAsString(archive, "content.xml");
				var metaXml = _zipHandlerService.GetEntryAsString(archive, "meta.xml");

				var metadata = _buildOdfMetadataService.BuildOdfMetadata(metaXml, _manager);

				var information = new DocumentInformation(fileType, document, content, metadata);

				return information;
			}
		}
	}
}