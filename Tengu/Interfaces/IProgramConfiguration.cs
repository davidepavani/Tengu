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
        HostConfiguration Hosts { get; set; }
        DownloadsConfiguration Downloads { get; set; }
        bool Save();
    }
}
