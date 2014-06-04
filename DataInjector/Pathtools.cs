using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DataInjector
{
    internal class PathTools
    {
        public static string TempFolderMaker(string path, string desiredPath = "\\Generated Reports\\temp")
        {
            var tempfolderpath = path + desiredPath;
            Directory.CreateDirectory(tempfolderpath);
            var info = new DirectoryInfo(path);
            foreach (var file in info.GetFiles("*", SearchOption.AllDirectories))
            {
                file.Attributes = FileAttributes.Normal;
            }
            foreach (var  folder in info.GetDirectories("*", SearchOption.AllDirectories))
            {
                folder.Attributes = FileAttributes.Normal;
            }

            return tempfolderpath;
        }

        public static string RemoveFileNameFromPath(string path)
        {
            var pathPiecesArray = path.Split(new[] {'\\'}, StringSplitOptions.None);
            var pathBuilder = new StringBuilder();
            var pathPieces = pathPiecesArray.ToList();
            pathPieces.RemoveRange(pathPieces.Count - 1, 1);

            foreach (var piece in pathPieces)
            {
                pathBuilder.Append(piece + "\\");
            }

            string returnPath = pathBuilder.ToString();
            returnPath = returnPath.Substring(0, returnPath.LastIndexOf("\\", StringComparison.Ordinal));
            return returnPath;
        }
    }
}