using Avalonia.Collections;
using FluentAvalonia.UI.Controls;
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
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Data;
using Tengu.Dialogs;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class LatestEpisodesControlViewModel : ViewModelBase
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.MainLogger);

        private AvaloniaList<LatestModel> latestEpisodesList = new();
        private int latestEpisodesOffset = 0;
        private Hosts selectedHost;
        private int currentPage;

        private bool loading = false;
        private bool canPrev = false;

        #region Properties
        public ICommand CmdNextPage { get; private set; }
        public ICommand CmdPrevPage { get; private set; }
        public ICommand CmdOpenAnimeCard { get; private set; }

        public AvaloniaList<LatestModel> LatestEpisodesList
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

                ProgramConfig.Hosts.Latest = SelectedHost;
                Initialize();
            }
        }
        public int LatestEpisodesOffset
        {
            get => latestEpisodesOffset;
            set
            {
                this.RaiseAndSetIfChanged(ref latestEpisodesOffset, value);
                CanPrev = value >= 10;

                CurrentPage = LatestEpisodesOffset / 10;
            }
        }
        public int CurrentPage
        {
            get => currentPage;
            set => this.RaiseAndSetIfChanged(ref currentPage, value);
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
            CmdNextPage = ReactiveCommand.Create(LatestNextPage);
            CmdPrevPage = ReactiveCommand.Create(LatestPrevPage);
            CmdOpenAnimeCard = ReactiveCommand.Create<LatestModel>(ShowAnimeCard);

            SelectedHost = ProgramConfig.Hosts.Latest;
        }

        public void Initialize()
        {
            TenguApi.CurrentHosts = new Hosts[] { SelectedHost };

            LatestEpisodesOffset = 0;

            RefreshLatestEpisodes();
        }

        private async void RefreshLatestEpisodes()
        {
            Loading = true;

            try
            {
                LatestEpisodesList.Clear();

                TenguResult<EpisodeModel[]> res = await TenguApi.GetLatestEpisodeAsync(LatestEpisodesOffset, LatestEpisodesOffset + 10);

                foreach (TenguResultInfo infoRes in res.Infos)
                {
                    if (!infoRes.Success)
                    {
                        InfoBar.AddMessage($"Latest Episodes Error ({infoRes.Host})",
                                   infoRes.Exception.Message,
                                   InfoBarSeverity.Error);

                        log.Error(infoRes.Exception, "RefreshLatestEpisodes >> GetLatestEpisodeAsync | Host: {host}", infoRes.Host);
                    }
                }

                foreach (EpisodeModel episode in res.Data)
                {
                    LatestEpisodesList.Add(new(episode));
                }
            }
            catch(Exception ex)
            {
                InfoBar.AddMessage("Latest Episodes Error",
                                   ex.Message,
                                   InfoBarSeverity.Error);
                log.Error(ex, "RefreshLatestEpisodes >> GetLatestEpisodeAsync");
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

        public async void ShowAnimeCard(LatestModel episode)
        {
            if (null != episode)
            {
                TenguResult<AnimeModel[]> res = await TenguApi.SearchAnimeAsync(episode.Episode.Title, 1);

                foreach (TenguResultInfo infoRes in res.Infos)
                {
                    if (!infoRes.Success)
                    {
                        InfoBar.AddMessage($"Show AnimeCard Error ({infoRes.Host})",
                                   infoRes.Exception.Message,
                                   InfoBarSeverity.Error);

                        log.Error(infoRes.Exception, "ShowAnimeCard >> SearchAnimeAsync | Host: {host}", infoRes.Host);
                        return; // FATAL
                    }
                }

                if(!res.Data.Any())
                {
                    InfoBar.AddMessage($"Show AnimeCard Error ({episode.Episode.Host})",
                                   $"Anime Card ({episode.Episode.Title}) Not found",
                                   InfoBarSeverity.Error);

                    log.Error("ShowAnimeCard >> SearchAnimeAsync | Host: {host} - Anime not found", episode.Episode.Host);
                    return; // FATAL
                }

                AnimeModel anime = res.Data[0];

                var dialog = new ContentDialog()
                {
                    Title = anime.Title,
                    CloseButtonText = "Close"
                };

                var viewModel = new AnimeCardDialogViewModel(dialog, anime, anime.Host);
                dialog.Content = new AnimeCardDialog()
                {
                    DataContext = viewModel
                };

                _ = await dialog.ShowAsync();
            }
        }
    }
}
