using System;
using RazorEngine;

// ReSharper disable CheckNamespace
namespace AntiShaun
// ReSharper restore CheckNamespace
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