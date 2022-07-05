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

        [Description("Dark Slate Blue")]
        darkslateblue = 0x483d8b,

        [Description("Deep Pink")]
        deeppink = 0xff1493,

        [Description("Dodger Blue")]
        dodgerblue = 0x1e90ff,

        [Description("Indigo")]
        indigo = 0x4b0082,

        [Description("Teal")]
        teal = 0x008080,

        [Description("Spring Green")]
        springgreen = 0x00ff7f,

        [Description("Salmon")]
        salmon = 0xfa8072,

        [Description("Sienna")]
        sienna = 0xa0522d,

        [Description("Peru")]
        peru = 0xcd853f,

        [Description("Lime")]
        lime = 0x00ff00,

        [Description("Dark Violet")]
        darkviolet = 0x9400d3,

        [Description("Coral")]
        coral = 0xff7f50,

        [Description("Dark Slate Grey")]
        darkslategrey = 0x2f4f4f,

        [Description("Gold")]
        gold = 0xffd700,

        [Description("Yellow Green")]
        yellowgreen = 0x9acd32
    }
}
