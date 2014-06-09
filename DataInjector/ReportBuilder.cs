using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Ionic.Zip;
using System.IO;
using ZipFile = Ionic.Zip.ZipFile;


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
                xmlContent = DataTools.InsertDate(contentStream);
            }


            File.WriteAllText(tempFilePath + "\\content.xml", xmlContent);
            var reportsFolderPath = tempFilePath.Substring(0,
                                                           tempFilePath.LastIndexOf("\\", StringComparison.Ordinal));

            var fileList = Directory.EnumerateFiles(tempFilePath);
            var reportPath = reportsFolderPath + "\\Report_1.odt";
            using (var output = new ZipOutputStream(reportPath))
            {
                var enumerable = fileList as string[] ?? fileList.ToArray();
                foreach (var filepath in enumerable.Where(filepath => filepath.EndsWith("mimetype")))
                {
                    using (var fs = new FileStream(filepath, FileMode.Open))
                    {
                        var entry = output.PutNextEntry(
                            filepath.Substring(filepath.LastIndexOf("\\", StringComparison.Ordinal)));
                        entry.CompressionMethod = CompressionMethod.None;
                        WriteExistingFile(fs, output);
                    }
                }
                foreach (var file in enumerable)
                {
                    using (var fs = new FileStream(file, FileMode.Open))
                    {
                        if (file.EndsWith("mimetype")) continue;
                        var entryString = file.Substring(file.LastIndexOf("\\", StringComparison.Ordinal));
                        var entry = output.PutNextEntry(entryString);

                        WriteExistingFile(fs, output);
                    }
                }
            }
            var zippedFile = new ZipFile(reportPath);
            var folders = new string[] {tempFilePath +"\\Configurations2", tempFilePath+ "\\META-INF",tempFilePath+ "\\Thumbnails"};
            foreach (var folder in folders)
            {
                zippedFile.AddDirectory(folder, folder.Substring(folder.LastIndexOf("\\", StringComparison.Ordinal)));
            }
            zippedFile.Save();
            zippedFile.Dispose();



            Directory.Delete(tempFilePath, true);
            return folderPath;
        }

        private static void WriteExistingFile(Stream input, ZipOutputStream output)
        {
            int n = -1;
            var buffer = new byte[2048];
            while ((n = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, n);
            }
        }
    }
}