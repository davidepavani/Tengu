using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Interfaces
{
    public interface IProgramConfiguration
    {
        bool IsDarkMode { get; set; }
        Hosts LatestHostSelected { get; set; }
        bool Save();
    }
}
