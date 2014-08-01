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
using System.IO;

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

			using (var reader = new StreamReader(zipEntry.Open()))
			{
				return reader.ReadToEnd();
			}
		}

		public virtual IZipEntry GetZipEntry(IZipArchive archive, string entryName)
		{
			var entry = archive.GetEntry(entryName);
			return entry;
		}
	}
}