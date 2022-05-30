using Avalonia.Collections;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;
using Tengu.Business.Commons;

namespace Tengu.Services
{
    public class TenguManagerService : ReactiveObject
    {
        private AvaloniaList<EpisodeModel> latestEpisodesList = new();
        private int latestEpisodesOffset = 0;

        private bool loading = false;

        private static ITenguApi tenguApi => Locator.Current.GetService<ITenguApi>();

        public AvaloniaList<EpisodeModel> LatestEpisodesList
        {
            get => latestEpisodesList;
            set => this.RaiseAndSetIfChanged(ref latestEpisodesList, value);
        }
        public int LatestEpisodesOffset
        {
            get => latestEpisodesOffset;
            set => this.RaiseAndSetIfChanged(ref latestEpisodesOffset, value);
        }
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }

        public void LatestNextPage()
        {
            LatestEpisodesOffset += 10;
            // log.Trace($"Next page >> {CurrentPage}");

            RefreshLatestEpisodes();
        }

        public void LatestPrevPage()
        {
            if (LatestEpisodesOffset >= 10) 
            {
                LatestEpisodesOffset -= 10;
                // log.Trace($"Prev page >> {CurrentPage}");

                RefreshLatestEpisodes();
            }
        }

        private async void RefreshLatestEpisodes()
        {
            Loading = true;

            try
            {
                LatestEpisodesList.Clear();

                foreach (EpisodeModel episode in await tenguApi.GetLatestEpisodeAsync(LatestEpisodesOffset, LatestEpisodesOffset + 10))
                {
                    LatestEpisodesList.Add(episode);
                }
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
