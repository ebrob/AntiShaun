using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;

namespace DataInjector
{
    public class ReportBuilder
    {
        //Path is the path to the template file
        public static string Main(string path)
        {
            var folderPath = PathTools.RemoveFileNameFromPath(path);

            var tempFilePath = PathTools.TempFolderMaker(folderPath);
            CompressionTools.UnzipToDirectory(path, tempFilePath);
            var contentPath = tempFilePath + "\\content.xml";
            string xmlContent;
            using (var contentStream = new FileStream(contentPath, FileMode.Open))
            {
                StreamReader contentReader = new StreamReader(contentStream);
                string contentString = contentReader.ReadToEnd();
                xmlContent = DataTools.InsertDate(contentString);
            }


            File.WriteAllText(tempFilePath + "\\content.xml", xmlContent);
            string reportsFolderPath = tempFilePath.Substring(0,
                                                              tempFilePath.LastIndexOf("\\", StringComparison.Ordinal));
            if (PermissionChecker.HasWritePermission(reportsFolderPath))
            {
                var filePath = reportsFolderPath + "\\Report_1.zip";
                ZipFile.CreateFromDirectory(tempFilePath,filePath );
            }
            

            Directory.Delete(tempFilePath, true);
            return folderPath;
        }
    }
}