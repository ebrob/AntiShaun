namespace DataInjector
{
    public class ReportBuilderService
    {
        private readonly DocumentDeconstructService _deconstructService;
        private readonly ReportTemplateService _templateService;
        private readonly DocumentReconstructService _reconstructService;

        public ReportBuilderService(DocumentDeconstructService deconstructService,
                                    ReportTemplateService templateService,
                                    DocumentReconstructService reconstructService)
        {
            _deconstructService = deconstructService;
            _templateService = templateService;
            _reconstructService = reconstructService;
        }

        //Path is the path to the template file
        public string DoStuff(string path, object model)
        {
            var templateContent = _deconstructService.Deconstruct(path);

            var xmlContent = _templateService.ApplyTemplate(templateContent, model);

            return _reconstructService.Reconstruct(
                _deconstructService.TempFolderPath,
                xmlContent);
        }
    }
}