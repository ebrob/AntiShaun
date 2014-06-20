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
        private readonly IFileSystemService _fileSystemService;

        public DocumentDeconstructService(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
        }


        public string UnzipAndGetContent(string pathToDocument)
        {
            var contentPath = _fileSystemService.UnzipAndGetContentPath(pathToDocument);
            TempFolderPath = _fileSystemService.TempFolderPath;
            var contentString = _fileSystemService.GetFileContentAsString(contentPath);

            return contentString;
        }

        public string TempFolderPath { get; private set; }

        public void Dispose()
        {
            Directory.Delete(TempFolderPath, true);
        }
    }
}