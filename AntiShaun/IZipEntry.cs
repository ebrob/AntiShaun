#region

using System.IO;
using System.IO.Compression;

#endregion

namespace AntiShaun
{
	public interface IZipEntry
	{
		void Delete();
		Stream Open();
	}

	public class ZipEntry : IZipEntry
	{
		private readonly ZipArchiveEntry _entry;

		public ZipEntry(ZipArchiveEntry entry)
		{
			_entry = entry;
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