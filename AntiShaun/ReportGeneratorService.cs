#region Apache License

// /*Copyright 2014 EventBooking.com, LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, 
// software distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and limitations under the License
// */

#endregion

#region

using System;
using System.IO;
using System.IO.Compression;
using RazorEngine.Templating;

#endregion

namespace AntiShaun
{
	public interface IReportGeneratorService
	{
		void BuildReport( ITemplate template, object model, Stream outputStream );
	}

	public class ReportGeneratorService : IReportGeneratorService
	{
		private readonly ITemplateService _templateService;
		private readonly IZipFactory _zipFactory;

		public ReportGeneratorService( IZipFactory zipFactory, ITemplateService templateService )
		{
			_zipFactory = zipFactory;
			_templateService = templateService;
		}


		public void BuildReport( ITemplate template, object model, Stream outputStream )
		{
			var original = template.OriginalDocument;
			var newByteArray = new byte[original.Length];
			Buffer.BlockCopy( original, 0, newByteArray, 0, Buffer.ByteLength( original ) );
			using( var interimStream = new MemoryStream( newByteArray ) )
			{
				using( var archive = _zipFactory.ZipArchiveFromStream( interimStream, ZipArchiveMode.Update ) )
				{
					var reportText = _templateService.Run( template.CachedTemplateIdentifier, model, null );
					reportText = reportText.Replace( "U+10FFFD", "@" );
					var content = archive.GetEntry( "content.xml" );
					content.Delete();
					content = archive.CreateEntry( "content.xml" );

					using( var writer = new StreamWriter( content.Open() ) )
					{
						writer.Write( reportText );
					}
				}
				interimStream.Seek( 0, SeekOrigin.Begin );
				interimStream.CopyTo( outputStream );
			}
		}
	}
}