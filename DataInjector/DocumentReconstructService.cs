using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Ionic.Zip;

namespace DataInjector
{
    public interface IDocumentReconstructService
    {
        string Reconstruct(string tempFolderPath, string xmlContent);
    }

    public class DocumentReconstructService : IDocumentReconstructService
    {

        private IFileSystem _fileSystem;
        public DocumentReconstructService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }



        public string Reconstruct(string tempFolderPath, string xmlContent)
        {
//TODO: Rewrite to remove dependency on file system: perform all actions in memory
            var contentPath = _fileSystem.Path.Combine(tempFolderPath, "content.xml");
            _fileSystem.File.WriteAllText(contentPath, xmlContent);

            var reportsFolderPath = tempFolderPath.Substring(0,
                                                             tempFolderPath.LastIndexOf("\\", StringComparison.Ordinal));

            var fileList = Directory.EnumerateFiles(tempFolderPath);


            var startPath = reportsFolderPath + "\\Report.odt";
            var reportPath = FileSystemService.NextAvailableFilename(startPath);

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