#region

using System;
using System.IO;
using System.IO.Compression;

#endregion

namespace AntiShaun
{
	public interface IZipHandlerService
	{
		OdfHandlerService.FileType GetFileType(IZipArchive file);
		String GetEntryAsString(IZipArchive archive, string entry);
		IZipEntry GetZipEntry(IZipArchive archive, string entryName);
	}

	public class ZipHandlerService : IZipHandlerService
	{
		public virtual OdfHandlerService.FileType GetFileType(IZipArchive file)
		{
			var mimetypeIdentifier = GetEntryAsString(file, "mimetype");
			switch (mimetypeIdentifier)
			{
				case "application/vnd.oasis.opendocument.spreadsheet":
					return OdfHandlerService.FileType.Ods;
				case "application/vnd.oasis.opendocument.text":
					return OdfHandlerService.FileType.Odt;
			}
			throw new NotSupportedException(
				"Unknown or unexpected file type. Only ODT and ODS files are supported as input at this time.");
		}

		public virtual String GetEntryAsString(IZipArchive archive, string entry)
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

		public virtual IZipEntry GetZipEntry(IZipArchive archive, string entryName)
		{
			var entry = archive.GetEntry(entryName);
			return entry;
		}
	}
}