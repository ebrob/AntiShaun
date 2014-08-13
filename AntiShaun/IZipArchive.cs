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
		private readonly List<IZipEntry> _entries;
		private readonly ZipArchive _zip;

		public ZipWrapper(Stream stream, ZipArchiveMode mode, bool leaveOpen)
		{
			_entries = new List<IZipEntry>();
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