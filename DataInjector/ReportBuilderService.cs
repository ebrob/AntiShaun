using System.IO;
using RazorEngine;

namespace DataInjector
{
    public class ReportBuilderService
    {
        private readonly IDataHandlerService _dataHandlerService;

        public ReportBuilderService(IDataHandlerService dataHandlerService)
        {
            _dataHandlerService = dataHandlerService;
        }


        public void BuildReport(Template template, object model, Stream outputStream)
        {
            string reportXml;



            using (var templateFileStream = _dataHandlerService.WrapBytesInStream(templateFile))
              {
                var zipFile = _dataHandlerService.ZipArchiveFromStream(templateFileStream);

                var fileType = _dataHandlerService.GetFileType(zipFile);

                var templateContentEntry = _dataHandlerService.GetZipEntry(zipFile, "content.xml");

                using (var templateContentStream = _dataHandlerService.OpenZipEntry(templateContentEntry))
                {
                    var templateContent = _dataHandlerService.ReadEntryStreamContentAsString(templateContentStream);

                    reportXml = _xmlService.ApplyTemplate(templateContent, model, fileType);
                }
            }
            var reportDocument = _dataHandlerService.Clone(templateFile);

            using (var memoryStream = _dataHandlerService.WrapBytesInStream(reportDocument))
            {
                using (var archive = _dataHandlerService.ZipArchiveFromStream(memoryStream))
                {
                    var oldEntry = _dataHandlerService.GetZipEntry(archive, "content.xml");
                    oldEntry.Delete();

                    var newEntry = archive.CreateEntry("content.xml");
                    using (var entryStream = newEntry.Open())
                    {
                        using (var streamWriter = new StreamWriter(entryStream))
                        {
                            streamWriter.Write(reportXml);
                        }
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                memoryStream.CopyTo(outputStream);
            }
        }


        private string RazorParseAndReinsertAtSigns(string template, object model)
        {
            var razorOutput = Razor.Parse(template, model);
            string returnValue = razorOutput.Replace("U+10FFFD", "@");


            return returnValue;
        }
    }
}