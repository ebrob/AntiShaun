#region

using System;
using System.IO;
using System.IO.Compression;

#endregion

namespace AntiShaun
{
	public interface IFileHandlerService
	{
		byte[] Copy(byte[] original);
		IZipArchive ZipArchiveFromStream(Stream stream, ZipArchiveMode mode = ZipArchiveMode.Read);
		IZipArchive ZipArchiveFromDocument(byte[] document, ZipArchiveMode mode = ZipArchiveMode.Read);
	}

	public class FileHandlerService : IFileHandlerService
	{
		public virtual byte[] Copy(byte[] original)
		{
			var newByteArray = new byte[original.Length];
			Buffer.BlockCopy(original, 0, newByteArray, 0, Buffer.ByteLength(original));
			return newByteArray;
		}

		public virtual IZipArchive ZipArchiveFromStream(Stream stream, ZipArchiveMode mode = ZipArchiveMode.Read)
		{
			var archive = new ZipWrapper(stream, mode, true);

			return archive;
		}

		public virtual IZipArchive ZipArchiveFromDocument(byte[] document, ZipArchiveMode mode = ZipArchiveMode.Read)
		{
			var stream = WrapBytesInStream(document); //ZipArchive disposes stream when disposed

			var archive = new ZipWrapper(stream, mode, true);

			return archive;
		}

		private Stream WrapBytesInStream(byte[] buffer)
		{
			var stream = new MemoryStream(buffer);
			return stream;
		}
	}
}