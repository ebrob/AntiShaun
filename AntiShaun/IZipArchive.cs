#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;

#endregion

namespace AntiShaun
{
	public interface IZipArchive : IDisposable
	{
		ZipArchiveMode Mode { get; }
		ReadOnlyCollection<IZipEntry> Entries { get; }
		IZipEntry CreateEntry(string entryName, CompressionLevel compressionLevel);
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
				_entries.Add(new ZipEntry(zipEntry, this));
			}
		}

		public ZipArchiveMode Mode
		{
			get { return _zip.Mode; }
		}

		public ReadOnlyCollection<IZipEntry> Entries
		{
			get { return _entries.AsReadOnly(); }
		}

		public IZipEntry CreateEntry(string entryName, CompressionLevel compressionLevel)
		{
			var entry = new ZipEntry(_zip.CreateEntry(entryName, compressionLevel), this);
			_entries.Add(entry);
			return entry;
		}

		public IZipEntry CreateEntry(string entryName)
		{
			var entry = new ZipEntry(_zip.CreateEntry(entryName),this);
			_entries.Add(entry);
			return entry;
		}

		public void Dispose()
		{
			_zip.Dispose();
		}

		public IZipEntry GetEntry(string entryName)
		{
			return new ZipEntry(_zip.GetEntry(entryName),this);
		}
	}
}