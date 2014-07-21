using System;
using RazorEngine;

namespace DataInjector
{


    public class RazorTemplateCompileService
    {
        public void Compile(Template template, String cacheName)
        {
            Razor.Compile(template.Content, cacheName);
            template.CachedTemplateIdentifier = cacheName;
        }

      
    }
}