using System;
using ZipFile = Ionic.Zip.ZipFile;


namespace DataInjector
{
    public interface IDecompressionService
    {
        void UnzipToDirectory(string path, string targetfolder);
        void Dispose();
    }

    public class DecompressionService:IDisposable, IDecompressionService
    {
        private ZipFile _file;
        public void UnzipToDirectory(string path, string targetfolder)
        {
            _file = ZipFile.Read(path);
            _file.ExtractAll(targetfolder);
            
        }

        public void Dispose()
        {
            _file.Dispose();
        }
    }
}