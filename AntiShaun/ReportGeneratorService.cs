using System.IO;
using System.IO.Compression;
using RazorEngine;

namespace AntiShaun //TODO: Refactor
{
    public class ReportGeneratorService
    {
        private readonly OdfHandlerService _odfHandlerService;

        public ReportGeneratorService(OdfHandlerService odfHandlerService)
        {
            _odfHandlerService = odfHandlerService;
        }


        public void BuildReport(Template template, object model, Stream outputStream)
        {
            outputStream.Write(template.OriginalDocument, 0, template.OriginalDocument.Length);
            var archive = _odfHandlerService.ZipArchiveFromStream(outputStream, ZipArchiveMode.Update);
            var reportText = Razor.Run(template.CachedTemplateIdentifier, model);
            reportText = reportText.Replace("U+10FFFD", "@");
            var meta = archive.GetEntry("meta.xml"); //TODO: Craft meta.xml? Or modify existing?
            var content = archive.GetEntry("content.xml");
            content.Delete();
            content = archive.CreateEntry("content.xml");
            using (var contentStream = content.Open())
            {
                using (var writer = new StreamWriter(contentStream))
                {
                    writer.Write(reportText);
                }
            }
        }
    }
}