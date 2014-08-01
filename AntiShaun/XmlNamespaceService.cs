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

using System;
using System.Collections.Generic;
using System.Xml;
using EnsureThat;

#endregion

namespace AntiShaun
{
	public interface IXmlNamespaceService: IXmlNamespaceResolver
	{
	}

	public class XmlNamespaceService : IXmlNamespaceService
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

		public IDictionary<string, string> GetNamespacesInScope(XmlNamespaceScope scope)
		{
			var namespaces = _manager.GetNamespacesInScope(scope);
			Ensure.That(namespaces, "Namespaces").IsNotNull();
			if (namespaces != null)
			{
				return namespaces;
			}
			throw new NullReferenceException("Namespaces are null");
		}

		public string LookupNamespace(string prefix)
		{
			return _manager.LookupNamespace(prefix);
		}

		public string LookupPrefix(string uri)
		{
			return _manager.LookupPrefix(uri);
		}
	}
}

