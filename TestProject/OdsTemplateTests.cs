#region Apache License

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

#endregion

#region

using AntiShaun;
using Moq;
using NUnit.Framework;

#endregion

namespace TestProject
{
	internal class OdsTemplateTests
	{
		private readonly Mock<DocumentInformation> _mockDocumentInformation = new Mock<DocumentInformation>();
		private readonly Mock<IXDocumentParserService> _mockXdocumentParserService = new Mock<IXDocumentParserService>();
		private readonly TemplateFactory _templateFactory = new TemplateFactory();
		private ITemplate _sut;


		[SetUp]
		public void SetUp ()
		{
			_mockDocumentInformation.SetupGet( x => x.FileType ).Returns( OdfHandlerService.FileType.Ods );
			_sut =_templateFactory.GenerateTemplate( _mockDocumentInformation.Object, new XmlNamespaceService(),
			                                   _mockXdocumentParserService.Object );
		}


		
	}
}