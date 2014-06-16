using System;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace DataInjector
{
    public class DocumentReconstructService
    {
        public string Reconstruct(string tempFolderPath, string xmlContent)
        {
            var contentPath = Path.Combine(tempFolderPath, "content.xml");
            File.WriteAllText(contentPath, xmlContent);

            var reportsFolderPath = tempFolderPath.Substring(0,
                                                             tempFolderPath.LastIndexOf("\\", StringComparison.Ordinal));

            var fileList = Directory.EnumerateFiles(tempFolderPath);
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
                        output.PutNextEntry(entryString);

                        WriteExistingFile(fs, output);
                    }
                }
                output.Flush();
                output.Close();
            }
            using (var zippedFile = new ZipFile(reportPath))
            {
                var folders = new[]
                    {
                        tempFolderPath + "\\Configurations2", tempFolderPath + "\\META-INF",
                        tempFolderPath + "\\Thumbnails"
                    };
                foreach (var folder in folders)
                {
                    zippedFile.AddDirectory(folder, folder.Substring(folder.LastIndexOf("\\", StringComparison.Ordinal)));
                }

                zippedFile.Save();

                Directory.Delete(tempFolderPath, true);
                return reportPath;
            }
        }

        private static void WriteExistingFile(Stream input, ZipOutputStream output)
        {
            int n;
            var buffer = new byte[2048];
            while ((n = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, n);
            }
        }
    }
}