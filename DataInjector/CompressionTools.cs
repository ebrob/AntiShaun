using System.IO;
using System.IO.Compression;

namespace DataInjector
{
    public class CompressionTools
    {
        public static ZipArchive UnzipToDirectory(string path, string targetfolder)
        {

            //Refactor to use DotNetZip for consistency (and fewer dependencies)? 
            //Yes, if this code needs refactored for some reason it should be refactored to use DNZ
            var stream = new FileStream(path, FileMode.Open);
            var archive = new ZipArchive(stream);
            archive.ExtractToDirectory(targetfolder);
            return archive;
        }
    }
}