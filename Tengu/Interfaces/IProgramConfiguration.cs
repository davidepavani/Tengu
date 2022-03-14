using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Interfaces
{
    public interface IProgramConfiguration
    {
        bool IsDarkMode { get; set; }
        bool Save();
    }
}
