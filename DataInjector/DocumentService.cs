using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AODL.Document.TextDocuments;
using EnsureThat;
using AODL.Document;
using AODL;

namespace DataInjector
{
    public interface IDocumentService
    {
        Stream WrapBytesInStream(byte[] buffer);
        ZipArchive ReadStreamAsArchive(Stream stream);
        ZipArchiveEntry GetArchiveEntry(ZipArchive archive, string entryName);
        Stream OpenArchiveEntry(ZipArchiveEntry entry);
        string ReadEntryStreamContentAsString(Stream entry);
    }

    public class DocumentService : IDocumentService
    {

       public Stream WrapBytesInStream(byte[] buffer)
       {
           var stream = new MemoryStream(buffer);
           return stream;
       }

        public ZipArchive ReadStreamAsArchive(Stream stream)
        {
            var archive = new ZipArchive(stream);
            return archive;
        }
        
        public ZipArchiveEntry GetArchiveEntry(ZipArchive archive, string entryName)
        {
            var entry = archive.GetEntry(entryName);
            return entry;
        }

        public Stream OpenArchiveEntry(ZipArchiveEntry entry)
        {
            return entry.Open();
        }

        public string ReadEntryStreamContentAsString(Stream entry)
        {
            using (var reader = new StreamReader(entry))
            {
                return reader.ReadToEnd();
            }
        }



    }
}