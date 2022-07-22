using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;

namespace Tengu.Configuration
{
    public class HostConfiguration
    {
        public TenguHosts Latest { get; set; } = TenguHosts.AnimeUnity;
        public TenguHosts Search { get; set; } = TenguHosts.None;
        public TenguHosts Calendar { get; set; } = TenguHosts.AnimeUnity;
    }
}
