using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Extensions;
using Avalonia.Media;

namespace Tengu.Models
{
    public class CalendarItem
    {
        public string Index { get; set; }
        public string Anime { get; set; }
        public IControl Image { get; set; }

        public CalendarItem(int index, CalendarEntryModel calendar)
        {
            Index = index.ToString();
            Anime = calendar.Anime;
            Image = calendar.Image.InitializeImage(Stretch.Uniform);
        }
    }
}
