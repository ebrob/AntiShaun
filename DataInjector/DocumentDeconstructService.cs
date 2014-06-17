using System;
using System.IO;

namespace DataInjector
{
    public interface IDocumentDeconstructService
    {
        string UnzipAndGetContent(string pathToDocument);
        string TempFolderPath { get; }
    }


    public class DocumentDeconstructService : IDisposable, IDocumentDeconstructService
    {
        private readonly IFileSystemService _fileSystemService = new FileSystemService();

        public string UnzipAndGetContent(string pathToDocument)
        {
            var contentPath = _fileSystemService.UnzipAndGetContentPath(pathToDocument);
            TempFolderPath = _fileSystemService.TempFolderPath;
            string contentString; 
            using (var content = new FileStream(contentPath, FileMode.Open))
            {
                var transformReader = new StreamReader(content);
                contentString = transformReader.ReadToEnd();
            }


            return contentString;
        }

        public string TempFolderPath { get; private set; }

        public void Dispose()
        {
            Directory.Delete(TempFolderPath, true);
        }
    }
}