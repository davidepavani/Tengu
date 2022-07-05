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
        public bool IsLightMode { get; set; } = false;
        public CustomColorModel AppColor { get; set; } = new(DefaultColors.aquamarine.GetEnumDescription(), $"#{(uint)DefaultColors.aquamarine:x6}");
    }
}
