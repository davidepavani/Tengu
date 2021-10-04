using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Enums
{
    public static class EnumExtension
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string GetControlNameFromEnum(this NavigateDestination navigate_destination)
        {
            switch (navigate_destination)
            {
                case NavigateDestination.About: return "AboutUserControl";
                case NavigateDestination.Options: return "SettingsUserControl";
                case NavigateDestination.Search: return "SearchUserControl";
                case NavigateDestination.UpcomingAnimes: return "UpcomingAnimesUserControl";
                case NavigateDestination.Calendar: return "CalendarUserControl";
                case NavigateDestination.Downloads: return "DownloadUserControl";
                case NavigateDestination.LatestEpisodes: return "LatestEpisodesUserControl";
                default: return null;
            }
        }
    }
}
