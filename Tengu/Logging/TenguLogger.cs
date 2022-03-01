using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;

namespace Tengu.Logging
{
    internal class TenguLogger 
    {
        public ILogger<TenguApi> Logger { get; set; }

        public TenguLogger()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.Configure((x) => { }));
            Logger = loggerFactory.CreateLogger<TenguApi>();
        }
    }
}
