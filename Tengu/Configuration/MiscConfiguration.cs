using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Enums;
using Tengu.Extensions;
using Tengu.Models;

namespace Tengu.Configuration
{
    public class MiscConfiguration
    {
        public bool IsDarkMode { get; set; } = true;
        public CustomColorModel AppColor { get; set; } = new();
    }
}
