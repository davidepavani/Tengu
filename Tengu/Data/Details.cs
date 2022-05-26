using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Data
{
    public static class Details
    {
        public const string ApplicationName = @"Tengu";
        public const string ApplicationDisplay = @"Tengu";

        public const string CompanyName = @"Tengu Business";

        private const string MajorVersion = "1"; // Manual Increment
        private const string MinorVersion = "1"; // Manual Increment
        private const string BuildVersion = "*"; // Auto Increment

        // AssemblyVersion Property
        public const string FullVersion = $"{MajorVersion}.{MinorVersion}.{BuildVersion}";

        // REAL Version Assembly 
        private static Version VersionAssembly => Assembly.GetExecutingAssembly().GetName().Version;

        #region Properties

        /// <summary>
        /// Build Date and Time
        /// </summary>
        public static string BuildDate => $"{new DateTime(2000, 1, 1).AddDays(VersionAssembly.Build).AddSeconds(VersionAssembly.Revision * 2)}";

        /// <summary>
        /// Display Application Version (Revision Version if Debug Mode)
        /// </summary>
        public static string DisplayVersion
        {
            get
            {
                return $"{VersionAssembly.Major}" +
                       $".{VersionAssembly.Minor.ToString().PadLeft(2, '0')}" +
                       $".{VersionAssembly.Build.ToString().PadLeft(2, '0')}"
#if DEBUG
                     + $" (Rev {VersionAssembly.Revision.ToString().PadLeft(2, '0')})"
#endif
                 ;
            }
        }
        #endregion
    }
}
