using System;
using System.IO;
using System.IO.Compression;

namespace DataInjector
{
    public interface IDataHandlerService
    {
        DocumentInformation BuildDocumentInformation(byte[] document, Type type);
    }

    public class DataHandlerService : IDataHandlerService
    {
        public enum FileType
        {
            Unknown,
            Odt,
            Ods
        }

        public DocumentInformation BuildDocumentInformation(byte[] document, Type type)
        {
            using (var stream = WrapBytesInStream(document))
            {
                using (var archive = ZipArchiveFromStream(stream))
                {
                    var fileType = GetFileType(archive);
                    var content = GetEntryAsString(archive, "content.xml");
                    var metadata = new OdfMetadata(archive.GetEntry("meta.xml"), type);
                    var information = new DocumentInformation(fileType, document, content, metadata);

                    return information;
                }
            }
        }


        public FileType GetFileType(ZipArchive file)
        {
            var mimetype = file.GetEntry("mimetype");
            using (var stream = mimetype.Open())
            {
                using (var reader = new StreamReader(stream))
                {
                    var mimetypeIdentifier = reader.ReadToEnd();
                    switch (mimetypeIdentifier)
                    {
                        case "application/vnd.oasis.opendocument.spreadsheet":
                            return FileType.Ods;
                        case "application/vnd.oasis.opendocument.text":
                            return FileType.Odt;
                    }
                    throw new NotSupportedException(
                        "Unknown or unexpected file type. Only ODT and ODS files are supported as input at this time.");
                }
            }
        }

        private Stream WrapBytesInStream(byte[] buffer)
        {
            var stream = new MemoryStream(buffer);
            return stream;
        }

        public ZipArchive ZipArchiveFromStream(Stream stream)
        {
            var archive = new ZipArchive(stream, ZipArchiveMode.Update, true);

            return archive;
        }

        public String GetEntryAsString(ZipArchive archive, string entry)
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