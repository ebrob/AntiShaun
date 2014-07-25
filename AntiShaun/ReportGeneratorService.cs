#region

using System.IO;
using System.IO.Compression;
using RazorEngine;

#endregion

namespace AntiShaun //TODO: Refactor
{
	public class ReportGeneratorService
	{
		private readonly IFileHandlerService _fileHandlerService;

		public ReportGeneratorService(IFileHandlerService fileHandlerService)
		{
			_fileHandlerService = fileHandlerService;
		}


		public void BuildReport(Template template, object model, Stream outputStream)
		{
			var newByteArray = _fileHandlerService.Copy(template.OriginalDocument);
			using (var interimStream = new MemoryStream(newByteArray))
			{
				using (var archive = _fileHandlerService.ZipArchiveFromStream(interimStream, ZipArchiveMode.Update))
				{
					var reportText = Razor.Run(template.CachedTemplateIdentifier, model);
					reportText = reportText.Replace("U+10FFFD", "@");
					//TODO: Craft meta.xml? Or modify existing?
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
				interimStream.Seek(0, SeekOrigin.Begin);
				interimStream.CopyTo(outputStream);
			}
		}
	}
}