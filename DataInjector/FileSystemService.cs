using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DataInjector
{
    public interface IFileSystemService
    {
        string TempFolderPath { get; }
        string UnzipAndGetContentPath(string pathToDocument);
        string RemoveFileNameFromPath(string path);
        string LocateContent(string path);
        string GetFileContentAsString(string contentPath);
    }

    public class FileSystemService : IFileSystemService
    {
        private readonly IDecompressionService _decompressionService;

        public FileSystemService(IDecompressionService decompressionService)
        {
            _decompressionService = decompressionService;
        }


        public string TempFolderPath { get; private set; }

        public string UnzipAndGetContentPath(string pathToDocument)
        {

            var folderPath = Path.GetDirectoryName(pathToDocument);

            TempFolderPath = CreateTempFolder(folderPath);

            _decompressionService.UnzipToDirectory(pathToDocument, TempFolderPath);

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

        private void NormalizeAttributes(string path)
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

        public string GetFileContentAsString(string contentPath)
        {
            using (var content = new FileStream(contentPath, FileMode.Open))
            {
                var transformReader = new StreamReader(content);
                var contentString = transformReader.ReadToEnd();
                return contentString;
            }
        }

//StackOverflow code.
        private const string NumberPattern = " ({0})";

        public static string NextAvailableFilename(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            return Path.HasExtension(path)
                       ? GetNextFilename(
                           path.Insert(
                               path.LastIndexOf(Path.GetExtension(path), comparisonType: StringComparison.Ordinal),
                               NumberPattern))
                       : GetNextFilename(path + NumberPattern);

            // Otherwise just append the pattern to the path and return next filename
        }

        private static string GetNextFilename(string pattern)
        {
            var tmp = string.Format(pattern, 1);
            if (tmp == pattern)
                throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                var pivot = (max + min)/2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }

        //end SO code
    }
}