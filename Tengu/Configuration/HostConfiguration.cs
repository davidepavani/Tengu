using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Configuration
{
    public class HostConfiguration
    {
        public Hosts Latest { get; set; } = Hosts.AnimeUnity;
        public Hosts Search { get; set; } = Hosts.AnimeUnity;
        public Hosts Calendar { get; set; } = Hosts.AnimeUnity;
    }
}
