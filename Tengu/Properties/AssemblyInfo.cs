using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tengu.Data;

[assembly: AssemblyTitle(Details.ApplicationName)]
[assembly: AssemblyCompany(Details.CompanyName)]
[assembly: AssemblyProduct(Details.ApplicationName)]

[assembly: AssemblyVersion(Details.FullVersion)]

#pragma warning disable CS7035 // The specified version string does not conform to the recommended format - major.minor.build.revision
[assembly: AssemblyFileVersion(Details.FullVersion)]
#pragma warning restore CS7035 // The specified version string does not conform to the recommended format - major.minor.build.revision