#region

using System;
using RazorEngine;

#endregion

namespace AntiShaun
{
	public interface ICompileService
	{
		void Compile(ITemplate template, String cacheName);
	}

	public class CompileService : ICompileService
	{
		public void Compile(ITemplate template, String cacheName)
		{
			Razor.Compile(template.Content, template.Meta.Type, cacheName);
			template.CachedTemplateIdentifier = cacheName;
		}
	}
}