using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Configuration;

namespace Tengu.Interfaces
{
    public interface IProgramConfiguration
    {
        MiscConfiguration Miscellaneous { get; set; }
        HostConfiguration TenguHosts { get; set; }
        DownloadsConfiguration Downloads { get; set; }
        bool Save();
    }
}
