using Avalonia.Collections;
using NLog;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Interfaces;
using Tengu.Logging;

namespace Tengu.ViewModels
{
    public class AnimeModelDialogViewModel : ReactiveObject
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.AnimeModelLoggerName);

        private readonly ITenguApi tenguApi;
        private readonly IDownloadManager downloadManager;

        private AvaloniaList<EpisodeModel> episodesList = new();
        private bool loading;

        public AnimeModel AnimeData { get; private set; }
        public Hosts Host { get; private set; }
        public ICommand DownloadEpisodeCommand { get; private set; }
        public AvaloniaList<EpisodeModel> EpisodesList
        {
            get => episodesList;
            set => this.RaiseAndSetIfChanged(ref episodesList, value);
        }
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }

        public AnimeModelDialogViewModel(AnimeModel anime, Hosts currentHost) : base()
        {
            log.Trace($"Anime Model dialog opening");

            tenguApi = Locator.Current.GetService<ITenguApi>();
            downloadManager = Locator.Current.GetService<IDownloadManager>();

            DownloadEpisodeCommand = ReactiveCommand.Create<EpisodeModel>(DownloadEpisode);

            AnimeData = anime;
            Host = currentHost;

            Task.Run(() => LoadEpisodes());
        }
        public AnimeModelDialogViewModel() { }

        private void DownloadEpisode(EpisodeModel episode)
        {
            if(episode != null)
            {
                log.Info($"Enqueued {episode.Title} >> {episode.Host}");
                downloadManager.EnqueueAnime(episode);
            }
        }

        private async Task LoadEpisodes()
        {
            Loading = true;

            EpisodesList.Clear();

            log.Debug($"Loading episodes started: {AnimeData.Title}");

            try
            {
                foreach(EpisodeModel ep in await tenguApi.GetEpisodesAsync(AnimeData.Id, Host))
                {
                    EpisodesList.Add(ep);
                }

                log.Trace($"Episodes Loaded: {EpisodesList.Count}");
            }
            catch(Exception ex)
            {
                // Logging ..
                log.Error(ex, $"Load Episodes Exteption: {AnimeData.Title}");
            }
            finally
            {
                Loading = false;

                log.Debug($"Loading episodes ended");
            }
        }
    }
}
