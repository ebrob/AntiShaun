using System.IO;
using System.IO.Compression;

namespace DataInjector
{
    public class ReportBuilderService
    {
        private readonly ReportTemplateService _reportTemplateService;
        private readonly IDataHandlerService _dataHandlerService;

        public ReportBuilderService(ReportTemplateService reportTemplateService, IDataHandlerService dataHandlerService)
        {
            _reportTemplateService = reportTemplateService;
            _dataHandlerService = dataHandlerService;
        }

        public void BuildReport(byte[] templateDocument, object model, Stream outputStream)
        {
            string reportXml;

            using (var templateFileStream = _dataHandlerService.WrapBytesInStream(templateDocument))
            {
                var zipFile = _dataHandlerService.ZipArchiveFromStream(templateFileStream);

                var templateContentEntry = _dataHandlerService.GetZipEntry(zipFile, "content.xml");

                using (var templateContentStream = _dataHandlerService.OpenZipEntry(templateContentEntry))
                {
                    var templateContent = _dataHandlerService.ReadEntryStreamContentAsString(templateContentStream);

                    reportXml = _reportTemplateService.ApplyTemplate(templateContent, model);
                }
            }
            var reportDocument = _dataHandlerService.Clone(templateDocument);
            //BUG: This document never changes! The document isn't being edited at all!

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
    }
}