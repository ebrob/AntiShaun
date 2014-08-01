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