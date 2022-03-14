using Tengu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

[assembly: AssemblyTitle(ProgramDetails.ApplicationName)]
[assembly: AssemblyCompany(ProgramDetails.CompanyName)]
[assembly: AssemblyProduct(ProgramDetails.ApplicationName)]

[assembly: AssemblyVersion(ProgramDetails.FullVersion)]

#pragma warning disable CS7035 // The specified version string does not conform to the recommended format - major.minor.build.revision
[assembly: AssemblyFileVersion(ProgramDetails.FullVersion)]
#pragma warning restore CS7035 // The specified version string does not conform to the recommended format - major.minor.build.revision