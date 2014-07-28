#region

using System;
using System.IO;
using System.IO.Compression;

#endregion

namespace AntiShaun
{
	public interface IZipEntry
	{
		IZipArchive Archive { get; }
		long CompressedLength { get; }
		string FullName { get; }
		DateTimeOffset LastWriteTime { get; set; }
		long Length { get; }
		string Name { get; }
		void Delete();
		Stream Open();
	}

	public class ZipEntry : IZipEntry
	{
		private readonly ZipArchiveEntry _entry;
		private readonly IZipArchive _archive;

		public ZipEntry(ZipArchiveEntry entry, IZipArchive archive)
		{
			_entry = entry;
			_archive = archive;
		}

		public IZipArchive Archive
		{
			get { return _archive; }
		}

		public long CompressedLength
		{
			get { return _entry.CompressedLength; }
		}

		public string FullName
		{
			get { return _entry.FullName; }
		}

		public DateTimeOffset LastWriteTime
		{
			get { return _entry.LastWriteTime; }
			set { _entry.LastWriteTime = value; }
		}

		public long Length
		{
			get { return _entry.Length; }
		}

		public string Name
		{
			get { return _entry.Name; }
		}

		public void Delete()
		{
			_entry.Delete();
		}

		public Stream Open()
		{
			return _entry.Open();
		}
	}
}