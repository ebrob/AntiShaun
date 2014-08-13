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

using System.Xml;
using AntiShaun;
using Moq;
using NUnit.Framework;
using RazorEngine;
using RazorEngine.Templating;
using ITemplate = AntiShaun.ITemplate;

#endregion

namespace TestProject
{
	internal class CompileServiceTests
	{
		private readonly Mock<ITemplateService> _templateService = new Mock<ITemplateService>();
		private const string CacheName = "cacheID";
		private readonly CompileService _sut = new CompileService();
		private const string TemplateString = "template";
		private ITemplate _template;

		[SetUp]
		public void Setup ()
		{

		}

		public void CompilesTemplate ()
		{
			_templateService.Setup( x => x.Compile( TemplateString, typeof( object ), CacheName ) );

			Razor.SetTemplateService( _templateService.Object );

		}
	}
}