using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Enums;
using Tengu.Extensions;

namespace Tengu.Models
{
    public class CustomColorModel
    {
        public string Name { get; set; }
        public string Hex { get; set; }

        public CustomColorModel() : this(DefaultColors.darkslateblue.GetEnumDescription(), $"#FF{(uint)DefaultColors.darkslateblue:x6}") { }
        public CustomColorModel(string name, string hex)
        {
            Name = name;
            Hex = hex.ToUpper();
        }
    }
}
