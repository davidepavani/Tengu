using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class CalendarItem
    {
        public string Index { get; set; }
        public string Anime { get; set; }
        public IControl Image { get; set; }
    }
}
