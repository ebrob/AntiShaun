// Copyright 2014 EventBooking.com, LLC
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

using System.Xml;

namespace AntiShaun
{
	public interface IOdfHandlerService
	{
		DocumentInformation BuildDocumentInformation( byte[] document );
	}

	public class OdfHandlerService : IOdfHandlerService
	{
		public enum FileType
		{
			Odt,
			Ods
		}

		private readonly IBuildOdfMetadataService _buildOdfMetadataService;

		private readonly IZipFactory _zipFactory;
		private readonly IXDocumentParserService _ixDocumentParserService;
		private readonly IXmlNamespaceResolver _manager;
		private readonly IZipHandlerService _zipHandlerService;

		public OdfHandlerService( IZipFactory zipFactory, IZipHandlerService zipHandlerService,
								  IBuildOdfMetadataService buildOdfMetadataService, IXmlNamespaceResolver xmlNamespaceService,
		                          IXDocumentParserService ixDocumentParserService )
		{
			_zipFactory = zipFactory;
			_zipHandlerService = zipHandlerService;
			_buildOdfMetadataService = buildOdfMetadataService;
			_manager = xmlNamespaceService;
			_ixDocumentParserService = ixDocumentParserService;
		}

		public virtual DocumentInformation BuildDocumentInformation( byte[] document )
		{
			var informationDocument = new byte[document.Length];
			document.CopyTo( informationDocument, 0 );

			using( var archive = _zipFactory.ZipArchiveFromDocument( document ) )
			{
				var fileType = _zipHandlerService.GetFileType( archive );
				var content = _zipHandlerService.GetEntryAsString( archive, "content.xml" );
				var metaXml = _zipHandlerService.GetEntryAsString( archive, "meta.xml" );

				var metadata = _buildOdfMetadataService.BuildOdfMetadata( metaXml, _manager, _ixDocumentParserService );

				var information = new DocumentInformation( fileType, informationDocument, content, metadata );

				return information;
			}
		}
	}
}