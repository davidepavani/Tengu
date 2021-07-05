using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tengu.Classes.Extensions
{
    public static class StringExtensions
    {
        public static string MakeValidFileName(this string source)
        {
            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(source, invalidRegStr, "_");
        }

        public static double ConvertToDouble(this string source)
        {
            if (source == null)
            {
                return 0;
            }
            else
            {
                double OutVal;
                double.TryParse(source, NumberStyles.Any, CultureInfo.InvariantCulture, out OutVal);

                if (double.IsNaN(OutVal) || double.IsInfinity(OutVal))
                {
                    return 0;
                }

                return OutVal;
            }
        }

        public static string NormalizeString(this string source)
        {
            return Regex.Replace(source, @"\t|\n|\r", string.Empty).Trim();
        }
    }
}
