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

namespace Tengu.ViewModels
{
    public class LatestEpisodesControlViewModel : ViewModelBase
    {
        private AvaloniaList<EpisodeModel> latestEpisodesList = new();
        private int latestEpisodesOffset = 0;
        private Hosts selectedHost = Hosts.AnimeSaturn;

        private bool loading = false;
        private bool canPrev = false;

        private readonly ITenguApi tenguApi;

        #region Properties
        public List<Hosts> HostsList { get; private set; }
        public AvaloniaList<EpisodeModel> LatestEpisodesList
        {
            get => latestEpisodesList;
            set => this.RaiseAndSetIfChanged(ref latestEpisodesList, value);
        }
        public Hosts SelectedHost
        {
            get => selectedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHost, value);
                Initialize();
            }
        }
        public int LatestEpisodesOffset
        {
            get => latestEpisodesOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref latestEpisodesOffset, value);
                CanPrev = value > 10;
            }
        }
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }
        public bool CanPrev
        {
            get => canPrev;
            set => this.RaiseAndSetIfChanged(ref canPrev, value);
        }
        #endregion

        public LatestEpisodesControlViewModel()
        {
            tenguApi = Locator.Current.GetService<ITenguApi>();


            HostsList = Enum.GetValues(typeof(Hosts)).Cast<Hosts>().ToList();
        }

        private void Initialize()
        {
            tenguApi.CurrentHosts = new Hosts[] { SelectedHost };
            LatestEpisodesOffset = 0;

            //ImageWidth = host.Equals(Hosts.AnimeUnity) ? 150 : 377.9;
            //BorderWidth = ImageWidth + 20;

            RefreshLatestEpisodes();
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
    }
}
