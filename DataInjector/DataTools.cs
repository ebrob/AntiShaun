using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace DataInjector
{
    internal class DataTools
    {
        public static string InsertDate(string content)
        {

            var newContent =content.Replace("@today", DateTime.Today.Date.ToString(CultureInfo.InvariantCulture));

            return newContent;

        }
    }
}