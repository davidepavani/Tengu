using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows;
using Tengu;

[assembly: SupportedOSPlatform("windows7.0")]
[assembly: SupportedOSPlatform("windows10")]

[assembly: Guid("6CCC103F-9225-4C06-A6D7-5BADE6CFC6A2")]

[assembly: AssemblyTitle(ProgramInfos.APPLICATION_NAME)]
[assembly: AssemblyCompany(ProgramInfos.COMPANY_NAME)]
[assembly: AssemblyProduct(ProgramInfos.APPLICATION_NAME)]
[assembly: AssemblyCopyright(ProgramInfos.COPYRIGHT)]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: AssemblyVersion(ProgramInfos.APPLICATION_VERSION)]
[assembly: AssemblyFileVersion(ProgramInfos.ASSEMBLY_FILE_VERSION)]