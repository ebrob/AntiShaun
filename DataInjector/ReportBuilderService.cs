using EnsureThat;

namespace DataInjector
{
    public class ReportBuilderService
    {
        private readonly IDocumentDeconstructService _deconstructService;
        private readonly IReportTemplateService _templateService;
        private readonly IDocumentReconstructService _reconstructService;

        public ReportBuilderService(IDocumentDeconstructService deconstructService,
                                    IReportTemplateService templateService,
                                    IDocumentReconstructService reconstructService)
        {
            _deconstructService = deconstructService;
            _templateService = templateService;
            _reconstructService = reconstructService;
        }

        //Path is the path to the template file
        public string DoStuff(string path, object model)
        {
            Ensure.That(path).IsNotNullOrWhiteSpace();
            var templateContent = _deconstructService.Deconstruct(path);
            
            var xmlContent = _templateService.ApplyTemplate(templateContent, model);

            return _reconstructService.Reconstruct(
                _deconstructService.TempFolderPath,
                xmlContent);
        }
    }
}