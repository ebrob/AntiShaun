#region

using System.Xml.Linq;

#endregion

namespace AntiShaun
{
	public interface IXDocumentParserService
	{
		XDocument Parse(string text);
	}

	public class XDocumentParserService : IXDocumentParserService
	{
		public XDocument Parse(string text)
		{
			return XDocument.Parse(text);
		}
	}
}