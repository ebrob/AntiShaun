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
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

#endregion

namespace AntiShaun
{
	public interface IZipArchive : IDisposable
	{
		IZipEntry CreateEntry(string entryName);
		IZipEntry GetEntry(string entryName);
	}

	public class ZipWrapper : IZipArchive
	{
		private readonly ZipArchive _zip;
		private readonly List<IZipEntry> _entries;

		public ZipWrapper(Stream stream, ZipArchiveMode mode, bool leaveOpen)
		{
			_entries=new List<IZipEntry>();
			_zip = new ZipArchive(stream, mode, leaveOpen);
			foreach (var zipEntry in _zip.Entries)
			{
				_entries.Add(new ZipEntry(zipEntry));
			}
		}

		public IZipEntry CreateEntry(string entryName)
		{
			var entry = new ZipEntry(_zip.CreateEntry(entryName));
			_entries.Add(entry);
			return entry;
		}

		public void Dispose()
		{
			_zip.Dispose();
		}

		public IZipEntry GetEntry(string entryName)
		{
			return new ZipEntry(_zip.GetEntry(entryName));
		}
	}
}