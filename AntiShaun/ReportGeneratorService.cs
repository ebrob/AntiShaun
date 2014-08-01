/*Copyright 2014 EventBooking.com, LLC

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License. 
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, 
software distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and limitations under the License
*/
#region

using System.IO;
using System.IO.Compression;
using RazorEngine;

#endregion

namespace AntiShaun 
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