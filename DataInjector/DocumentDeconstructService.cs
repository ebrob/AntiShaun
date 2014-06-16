using System;
using System.IO;

namespace DataInjector
{
    public class DocumentDeconstructService : IDisposable
    {
        public string Deconstruct(string pathToDocument)
        {
            var folderPath = FileSystem.RemoveFileNameFromPath(pathToDocument);
            TempFolderPath = FileSystem.CreateTempFolder(folderPath);
            using (var decompressionService = new DecompressionService())
            {
                decompressionService.UnzipToDirectory(pathToDocument, TempFolderPath);
            }
            var contentPath = FileSystem.LocateContent(TempFolderPath);

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