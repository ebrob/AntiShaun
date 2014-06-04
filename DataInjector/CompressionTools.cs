using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInjector
{
    public class CompressionTools
    {
        public static ZipArchive UnzipToDirectory(string path, string targetfolder)
        {
            var stream = new FileStream(path, FileMode.Open);
            var archive = new ZipArchive(stream);
            archive.ExtractToDirectory(targetfolder);
            return archive;
        }
    }
}