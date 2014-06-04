using System;
using System.Globalization;
using System.IO;
using System.Text;
using RazorEngine;
namespace DataInjector
{
    internal class DataTools
    {
        public static string InsertDate(string content)
        {
            string template = "@Model.Name";
            var model = new {Date = DateTime.Today};
            return Razor.Parse(template, model);


        }
    }
}