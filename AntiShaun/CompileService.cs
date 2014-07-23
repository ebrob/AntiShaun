using System;
using RazorEngine;

namespace AntiShaun
{
	public class CompileService
	{
		public void Compile(Template template, String cacheName)
		{
			Razor.Compile(template.Content, template.Meta.Type, cacheName);
			template.CachedTemplateIdentifier = cacheName;
		}
	}
}