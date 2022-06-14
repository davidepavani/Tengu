using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class CalendarDayModel
    {
        public string Day { get; set; }

        public List<CalendarItem> Animes { get; set; }
    }
}
