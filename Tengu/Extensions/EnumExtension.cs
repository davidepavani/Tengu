using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Extensions
{
    public static class EnumExtension
    {
        public static List<T> ToList<T>() where T : Enum
            => Enum.GetValues(typeof(T))
                   .Cast<T>()
                   .ToList();
    }
}
