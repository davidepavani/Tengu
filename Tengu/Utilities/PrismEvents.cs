using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Models;

namespace Tengu.Utilities
{
    public class PrismEvents
    {
        public class RemoveAnimeToDownloadListEvent : PubSubEvent<AnimeData> { }

        public class AddAnimeToDownloadListEvent : PubSubEvent<AnimeData> { }

        public class AddAnimeToDownloadHistoryEvent : PubSubEvent<HistoryData> { }

        public class ExecuteSearchEvent : PubSubEvent<string> { }

        public class DownloadEvent : PubSubEvent<bool> { }

        public class RefreshingCalendarEvent : PubSubEvent<bool> { }

        public class RefreshCalendarEvent : PubSubEvent { }

        public class AbortAllDownloadsEvent : PubSubEvent { }
    }
}
