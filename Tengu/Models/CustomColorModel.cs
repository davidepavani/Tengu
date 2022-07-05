using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class CustomColorModel
    {
        public string Name { get; private set; }
        public string Hex { get; private set; }

        public CustomColorModel(string name, string hex)
        {
            Name = name;
            Hex = hex.ToUpper();
        }
    }
}
