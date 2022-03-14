using Avalonia.Collections;
using NLog;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Extensions;
using Tengu.Interfaces;
using Tengu.Logging;
using Tengu.Utilities;

namespace Tengu.ViewModels
{
    public class LatestEpisodesPageViewModel : ReactiveObject
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.LatestLoggerName);

        private readonly ITenguApi tenguApi;
        private readonly IDownloadManager downloadManager;

        private AvaloniaList<EpisodeModel> episodeList = new(); 
        private Hosts selectedHost = Hosts.AnimeSaturn; 
        private bool loadingAnimes = false;
        private int currentPage = 0;
        private int offset = 0;
        private double imageWidth = 377.9;
        private double borderWidth = 397.9;

        private bool canPrev = false;

        #region Properties
        public Interaction<AnimeModelDialogViewModel, object> ShowAnimeModelDialog { get; }
        public ICommand OpenAnimeCardCommand { get; private set; }
        public ICommand DownloadEpisodeCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand PrevPageCommand { get; private set; }
        public List<Hosts> HostList { get; private set; }
        public bool CanPrev
        {
            get => canPrev;
            set => this.RaiseAndSetIfChanged(ref canPrev, value);
        }
        public bool LoadingAnimes
        {
            get => loadingAnimes;
            set => this.RaiseAndSetIfChanged(ref loadingAnimes, value);
        }
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                this.RaiseAndSetIfChanged(ref currentPage, value);
                CanPrev = !value.Equals(0);
            }
        }
        public AvaloniaList<EpisodeModel> EpisodeList
        {
            get => episodeList;
            set => this.RaiseAndSetIfChanged(ref episodeList, value);
        }
        public Hosts SelectedHost
        {
            get => selectedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHost, value);

                Initialize(value);
            }
        }
        public double ImageWidth
        {
            get => imageWidth;
            set => this.RaiseAndSetIfChanged(ref imageWidth, value);
        }
        public double BorderWidth
        {
            get => borderWidth;
            set => this.RaiseAndSetIfChanged(ref borderWidth, value);
        }
        #endregion

        public LatestEpisodesPageViewModel()
        {
            HostList = new();
            HostList.Add(Hosts.AnimeSaturn);
            HostList.Add(Hosts.AnimeUnity);

            ShowAnimeModelDialog = new();

            tenguApi = Locator.Current.GetService<ITenguApi>();
            downloadManager = Locator.Current.GetService<IDownloadManager>();

            NextPageCommand = ReactiveCommand.Create(NextPage);
            PrevPageCommand = ReactiveCommand.Create(PrevPage);

            OpenAnimeCardCommand = ReactiveCommand.CreateFromTask<EpisodeModel>(OpenAnimeDialog);
            DownloadEpisodeCommand = ReactiveCommand.Create<EpisodeModel>(DownloadEpisode);

            Initialize(SelectedHost);
        }

        private void DownloadEpisode(EpisodeModel episode)
        {
            if(episode != null)
            {
                log.Info($"Enqueued {episode.Title} >> {episode.Host}");
                downloadManager.EnqueueAnime(episode);
            }
        }

        private async Task OpenAnimeDialog(EpisodeModel episode)
        {
            if(null != episode)
            {
                try
                {
                    log.Info($"Opening Anime Card {episode.Title} >> {episode.Host}");

                    AnimeModel anime = (await tenguApi.SearchAnimeAsync(episode.Title, 1))[0];
                    var res = await ShowAnimeModelDialog.Handle(new AnimeModelDialogViewModel(anime, SelectedHost));
                }
                catch (Exception ex)
                {
                    log.Error(ex, $"Opening Anime Card Exception >> {episode.Title} | {episode.Host}");
                }
            }
        }

        public void PrevPage()
        {
            if (CanPrev)
            {
                CurrentPage -= 1;
                offset -= 10;

                log.Trace($"Prev page >> {CurrentPage}");

                _ = LoadAnimes();
            }
        }
        public void NextPage()
        {
            CurrentPage += 1;
            offset += 10;

            log.Trace($"Next page >> {CurrentPage}");

            _ = LoadAnimes();
        }

        private void Initialize(Hosts host)
        {
            tenguApi.CurrentHosts = new Hosts[] { host };
            CurrentPage = 0;
            offset = 0;

            ImageWidth = host.Equals(Hosts.AnimeUnity) ? 150 : 377.9;
            BorderWidth = ImageWidth + 20;

            _ = LoadAnimes();
        }

        private async Task LoadAnimes()
        {
            LoadingAnimes = true;
            EpisodeList.Clear();

            log.Trace($"Loading animes..");

            foreach (EpisodeModel episode in await tenguApi.GetLatestEpisodeAsync(offset, offset + 10))
            {
                EpisodeList.Add(episode);
            }

            log.Trace($"Loaded animes >> Episodes: {EpisodeList.Count}, Offset: {offset}");

            CanPrev = !CurrentPage.Equals(0) && EpisodeList.Count > 0;
            LoadingAnimes = false;
        }
    }
}
