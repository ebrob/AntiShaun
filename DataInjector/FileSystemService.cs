using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DataInjector
{
    internal interface IFileSystemService
    {
        string TempFolderPath { get; }
        string UnzipAndGetContentPath(string pathToDocument);
        string RemoveFileNameFromPath(string path);
        string LocateContent(string path);
    }

    internal class FileSystemService : IFileSystemService
    {
        public string TempFolderPath { get; private set; }


        public string UnzipAndGetContentPath(string pathToDocument)
        {
            var folderPath = RemoveFileNameFromPath(pathToDocument);
            TempFolderPath = CreateTempFolder(folderPath);
            using (var decompressionService = new DecompressionService())
            {
                decompressionService.UnzipToDirectory(pathToDocument, TempFolderPath);
            }

            return LocateContent(TempFolderPath);
        }


        public string RemoveFileNameFromPath(string path)
        {
            var pathPiecesArray = path.Split(new[] {'\\'}, StringSplitOptions.None);
            var pathBuilder = new StringBuilder();
            var pathPieces = pathPiecesArray.ToList();
            pathPieces.RemoveRange(pathPieces.Count - 1, 1);
            foreach (var piece in pathPieces)
            {
                pathBuilder.Append(piece + "\\");
            }
            var returnPath = pathBuilder.ToString();
            returnPath = returnPath.Substring(0, returnPath.LastIndexOf("\\", StringComparison.Ordinal));
            return returnPath;
        }


        private string CreateTempFolder(string path, string extension = "\\Generated Reports\\temp")
        {
            TempFolderPath = path + extension;

            Directory.CreateDirectory(TempFolderPath);

            NormalizeAttributes(path);

            return TempFolderPath;
        }

        private static void NormalizeAttributes(string path)
        {
            var info = new DirectoryInfo(path);

            foreach (var file in info.GetFiles("*", SearchOption.AllDirectories))
            {
                file.Attributes = FileAttributes.Normal;
            }

            foreach (var  folder in info.GetDirectories("*", SearchOption.AllDirectories))
            {
                folder.Attributes = FileAttributes.Normal;
            }
        }


        public string LocateContent(string path)
        {
            return path + "\\content.xml";
        }
    }
}