#region

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using EnsureThat;

#endregion

namespace AntiShaun
{


	public class XmlNamespaceService : IXmlNamespaceResolver
	{
		private readonly XmlNamespaceManager _manager;

		public XmlNamespaceService()
		{
			var table = new NameTable();
			_manager = new XmlNamespaceManager(table);
			_manager.AddNamespace("text", "urn:oasis:names:tc:opendocument:xmlns:text:1.0");
			_manager.AddNamespace("script", "urn:oasis:names:tc:opendocument:xmlns:script:1.0");
			_manager.AddNamespace("office", "urn:oasis:names:tc:opendocument:xmlns:office:1.0");
			_manager.AddNamespace("table", "urn:oasis:names:tc:opendocument:xmlns:table:1.0");
			_manager.AddNamespace("meta", "urn:oasis:names:tc:opendocument:xmlns:meta:1.0");
		}

		[ExcludeFromCodeCoverage]
		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			return _manager.GetNamespacesInScope(scope);
		}

		[ExcludeFromCodeCoverage]
		public string LookupNamespace(string prefix)
		{
			return _manager.LookupNamespace(prefix);
		}

		[ExcludeFromCodeCoverage]
		public string LookupPrefix(string uri)
		{
			return _manager.LookupPrefix(uri);
		}
	}
}