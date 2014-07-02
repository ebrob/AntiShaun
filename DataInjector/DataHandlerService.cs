using System;
using System.IO;
using System.IO.Compression;

namespace DataInjector
{
    public interface IDataHandlerService
    {
        Stream WrapBytesInStream(byte[] buffer);
        ZipArchive ZipArchiveFromStream(Stream stream);
        ZipArchiveEntry GetZipEntry(ZipArchive archive, string entryName);
        Stream OpenZipEntry(ZipArchiveEntry entry);
        string ReadEntryStreamContentAsString(Stream entry);
        Byte[] Clone(byte[] original);
    }

    public class DataHandlerService : IDataHandlerService
    {
        //TODO: Fluent interface?

        public Stream WrapBytesInStream(byte[] buffer)
        {
            var stream = new MemoryStream(buffer);
            return stream;
        }

        public ZipArchive ZipArchiveFromStream(Stream stream)
        {
            var archive = new ZipArchive(stream, ZipArchiveMode.Update, true);

            return archive;
        }


        public ZipArchiveEntry GetZipEntry(ZipArchive archive, string entryName)
        {
            var entry = archive.GetEntry(entryName);
            return entry;
        }

        public Stream OpenZipEntry(ZipArchiveEntry entry)
        {
            var stream = entry.Open();
            return stream;
        }

        public string ReadEntryStreamContentAsString(Stream entry)
        {
            using (var reader = new StreamReader(entry))
            {
                
                string contentString = reader.ReadToEnd();
                return contentString;
            }
        }

        public Byte[] Clone(byte[] original)
        {
            var newByteArray = new byte[original.Length];
            Buffer.BlockCopy(original, 0, newByteArray, 0, Buffer.ByteLength(original));
            return newByteArray;
        }
    }
}