using System;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace AntiShaun
{
	public interface IOdfHandlerService
	{
		DocumentInformation BuildDocumentInformation(byte[] document);
		ZipArchive ZipArchiveFromStream(Stream stream, ZipArchiveMode mode = ZipArchiveMode.Read);
	}

	public class OdfHandlerService : IOdfHandlerService
	{
		public enum FileType
		{
			Unknown,
			Odt,
			Ods
		}

		private readonly XmlNamespaceManager _manager;

		public OdfHandlerService()
		{
			var table = new NameTable();
			_manager = new XmlNamespaceManager(table);
			_manager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
			_manager.AddNamespace("script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
			_manager.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
			_manager.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
			_manager.AddNamespace("meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
		}

		public DocumentInformation BuildDocumentInformation(byte[] document)
		{
			using (var stream = WrapBytesInStream(document))
			{
				using (var archive = ZipArchiveFromStream(stream))
				{
					var fileType = GetFileType(archive);
					var content = GetEntryAsString(archive, "content.xml");

					var metaXml = GetEntryAsString(archive, "meta.xml");
					var metadata = new OdfMetadata(metaXml, _manager);


					var information = new DocumentInformation(fileType, document, content, metadata);

					return information;
				}
			}
		}


		private FileType GetFileType(ZipArchive file)
		{
			var mimetype = file.GetEntry("mimetype");
			using (var stream = mimetype.Open())
			{
				using (var reader = new StreamReader(stream))
				{
					var mimetypeIdentifier = reader.ReadToEnd();
					switch (mimetypeIdentifier)
					{
						case "application/vnd.oasis.opendocument.spreadsheet":
							return FileType.Ods;
						case "application/vnd.oasis.opendocument.text":
							return FileType.Odt;
					}
					throw new NotSupportedException(
						"Unknown or unexpected file type. Only ODT and ODS files are supported as input at this time.");
				}
			}
		}

		private Stream WrapBytesInStream(byte[] buffer)
		{
			var stream = new MemoryStream(buffer);
			return stream;
		}

		public ZipArchive ZipArchiveFromStream(Stream stream, ZipArchiveMode mode = ZipArchiveMode.Read)
		{
			var archive = new ZipArchive(stream, mode, true);

			return archive;
		}

		private String GetEntryAsString(ZipArchive archive, string entry)
		{
			var zipEntry = GetZipEntry(archive, entry);
			using (var stream = zipEntry.Open())
			{
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		private ZipArchiveEntry GetZipEntry(ZipArchive archive, string entryName)
		{
			var entry = archive.GetEntry(entryName);
			return entry;
		}
	}
}