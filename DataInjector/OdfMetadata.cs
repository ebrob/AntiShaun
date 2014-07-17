using System;
using System.IO.Compression;

namespace DataInjector
{
    public class OdfMetadata
    {
        private ZipArchiveEntry _entry;
        private readonly Type _type;
        public Type Type
        {
            get { return _type; }
        }

        public OdfMetadata(ZipArchiveEntry entry, Type type)
        {
            _entry = entry;
            _type = type;
        }
    }
}