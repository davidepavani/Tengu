using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Enums;
using Tengu.Extensions;
using Tengu.Models;

namespace Tengu.Data
{
    public class Misc
    {
        public static IEnumerable<CustomColorModel> LoadCustomDefaultColors()
        {
            foreach (DefaultColors color in EnumExtension.ToList<DefaultColors>())
            {
                yield return new CustomColorModel(color.GetEnumDescription(), $"#FF{(uint)color:x6}");
            }
        }
    }
}
