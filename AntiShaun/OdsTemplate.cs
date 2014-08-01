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

using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using EnsureThat;

#endregion

namespace AntiShaun
{
	internal class OdsTemplate : Template
	{
		public OdsTemplate(DocumentInformation documentInformation, IXmlNamespaceService xmlNamespaceService)
			: base(documentInformation, xmlNamespaceService)
		{
			HandleConditionals();
			ReplaceComments();
		}

		private void HandleConditionals()
		{
			var targetComments =
				DocumentContent.XPathSelectElements(@"//text:span", Manager).ToList();

			foreach (
				var comment in
					targetComments.Where(script => script.Value.Contains("foreach") || script.Value.Contains("if")))
			{
				CreateControlFlowFromComment(comment);
			}
		}

		private void CreateControlFlowFromComment(XElement comment)
		{
			var row = comment.XPathSelectElement("./ancestor::table:table-row", Manager);
			var commentValue = comment.Value.Replace("U+10FFFD", "@");

			var beforeNode = new XText(commentValue + "{");

			var afterNode = new XText("}");

			row.AddBeforeSelf(beforeNode);
			row.AddAfterSelf(afterNode);
			comment.Remove();
		}


		private void ReplaceComments()
		{
			var targetCells = DocumentContent.XPathSelectElements(@"//office:annotation/..", Manager).ToList();
			foreach (var cell in targetCells)
			{
				var comment = cell.XPathSelectElement(@"./office:annotation/text:p", Manager);
				var preparedValue = comment.Value.Replace("U+10FFFD", "@");
				var parent = comment.Parent;
				Ensure.That(parent).IsNotNull();
				if (parent != null) parent.Remove();

				XNamespace textNs = "urn:oasis:names:tc:opendocument:xmlns:text:1.0";
				var content = new XElement(textNs + "p") {Value = preparedValue};
				cell.Add(content);
			}
		}
	}
}