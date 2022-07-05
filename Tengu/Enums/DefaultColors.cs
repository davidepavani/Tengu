using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Enums
{
    // https://docs.microsoft.com/en-us/power-platform/power-fx/reference/function-colors
    // https://www.w3.org/wiki/CSS/Properties/color/keywords

    public enum DefaultColors : uint
    {
        [Description("Dark Orchid")]
        darkorchid = 0x9932cc,

        [Description("Crimson")]
        crimson = 0xdc143c,

        [Description("Dark Orange")]
        darkorange = 0xff8c00,
    }
}
