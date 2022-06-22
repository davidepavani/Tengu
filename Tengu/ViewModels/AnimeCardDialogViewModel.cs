using Avalonia.Collections;
using FluentAvalonia.UI.Controls;
using NLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Data;

namespace Tengu.ViewModels
{
    public class AnimeCardDialogViewModel : ViewModelBase
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.MainLogger);

        private readonly ContentDialog dialog;

        private AvaloniaList<EpisodeModel> episodesList = new();
        private int episodesCount = 0;
        private bool loading;

        #region Properties
        public AnimeModel AnimeData { get; private set; }
        public Hosts Host { get; private set; }
        public ICommand DownloadEpisodeCommand { get; private set; }
        public AvaloniaList<EpisodeModel> EpisodesList
        {
            get => episodesList;
            set => this.RaiseAndSetIfChanged(ref episodesList, value);
        }
        public int EpisodesCount
        {
            get => episodesCount;
            set => this.RaiseAndSetIfChanged(ref episodesCount, value);
        }
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }
        #endregion

        public AnimeCardDialogViewModel(ContentDialog dialog, AnimeModel anime, Hosts currentHost)
        {
            if (dialog is null)
                throw new ArgumentNullException(nameof(dialog));

            this.dialog = dialog;
            AnimeData = anime;
            Host = currentHost;

            DownloadEpisodeCommand = ReactiveCommand.Create<EpisodeModel>(DownloadEpisode);

            LoadEpisodes();
        }

        private void DownloadEpisode(EpisodeModel episode)
        {
            if (episode != null)
            {
                log.Info($"Enqueue {episode.Title} | Episode {episode.EpisodeNumber} | Host {episode.Host}");
                DwnldService.EnqueueAnime(episode);
            }
        }

        private async void LoadEpisodes()
        {
            Loading = true;
            EpisodesCount = 0;

            try
            {
                EpisodesList.Clear();

                foreach (EpisodeModel ep in (await TenguApi.GetEpisodesAsync(AnimeData.Id, Host)).Data)
                {
                    EpisodesList.Add(ep);
                }

                EpisodesCount = EpisodesList.Count;
            }
            catch (Exception ex)
            {
                InfoBar.AddMessage("Anime Card Error",
                                   ex.Message,
                                   InfoBarSeverity.Error);
                log.Error(ex, "LoadEpisodes >> GetEpisodesAsync");
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
