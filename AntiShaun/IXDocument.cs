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

using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

#endregion

namespace AntiShaun
{
	public interface IXDocumentWrapper
	{
		string BaseUri { get; }
		XDocument Document { get; }
		XElement Parent { get; }
		XElement Root { get; }
		bool HasLineInfo ();
		void Save( string fileName );
		void Save( string fileName, SaveOptions options );
	}

	public  class XDocumentWrapper : IXDocumentWrapper
	{
		private readonly XDocument _document;

		public XDocumentWrapper( XDocument document )
		{
			_document = document;
		}

		public string BaseUri
		{
			get { return _document.BaseUri; }
		}

		public XDocument Document
		{
			get { return _document.Document; }
		}

		public XElement Parent
		{
			get { return _document.Parent; }
		}

		public bool HasLineInfo ()
		{
			return ( (IXmlLineInfo) _document ).HasLineInfo();
		}

		public XElement Root
		{
			get { return _document.Root; }
		}

		public void Save( string fileName )
		{
			_document.Save( fileName );
		}

		public void Save( string fileName, SaveOptions options )
		{
			_document.Save( fileName, options );
		}
	}
}