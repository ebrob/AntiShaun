#region

using System;
using System.IO;
using System.IO.Compression;
using RazorEngine;
using RazorEngine.Templating;

#endregion

namespace AntiShaun
{
	public class ReportGeneratorService
	{
		private readonly IZipFactory _zipFactory;

		public ReportGeneratorService(IZipFactory zipFactory)
		{
			_zipFactory = zipFactory;
		}


		public void BuildReport(Template template, object model, Stream outputStream, ITemplateService templateService)
		{
			byte[] original = template.OriginalDocument;
			var newByteArray1 = new byte[original.Length];
			Buffer.BlockCopy(original, 0, newByteArray1, 0, Buffer.ByteLength(original));
			var newByteArray = newByteArray1;
			using (var interimStream = new MemoryStream(newByteArray))
			{
				using (var archive = _zipFactory.ZipArchiveFromStream(interimStream, ZipArchiveMode.Update))
				{
					var reportText = templateService.Run(template.CachedTemplateIdentifier, model, null);
					reportText = reportText.Replace("U+10FFFD", "@");
					var content = archive.GetEntry("content.xml");
					content.Delete();
					content = archive.CreateEntry("content.xml");

					using (var writer = new StreamWriter(content.Open()))
					{
						writer.Write(reportText);
					}
				}
				interimStream.Seek(0, SeekOrigin.Begin);
				interimStream.CopyTo(outputStream);
			}
		}
	}
}