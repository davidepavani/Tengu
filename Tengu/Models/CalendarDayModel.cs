using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons.Objects;

namespace Tengu.Models
{
    public class CalendarDayModel
    {
        public string Day { get; set; }

        public List<CalendarItem> Animes { get; set; }

        public WeekDays WeekDay { get; private set; }

        public CalendarDayModel(WeekDays day)
        {
            WeekDay = day;
            Day = day == WeekDays.None ? "OTHERS" : day.ToString().ToUpper();
            Animes = new();
        }
    }
}
