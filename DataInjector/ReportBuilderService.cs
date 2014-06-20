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

        public string InjectData(string pathToTemplate, object model)
        {//TODO: Refactor to pass zip archive through, so we don't need a temp folder.
            Ensure.That(pathToTemplate).IsNotNullOrWhiteSpace();
            var templateContent = _deconstructService.UnzipAndGetContent(pathToTemplate);
            
            var xmlContent = _templateService.ApplyTemplate(templateContent, model);

            return _reconstructService.Reconstruct(
                _deconstructService.TempFolderPath,
                xmlContent);
        }
    }
}