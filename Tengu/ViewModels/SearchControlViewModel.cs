using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using ReactiveUI;
using System.Windows.Input;
using Avalonia.Collections;
using Tengu.Extensions;
using Tengu.Models;
using Tengu.Data;
using NLog;
using Tengu.Business.API;
using FluentAvalonia.UI.Controls;
using Tengu.Business.Commons.Objects;
using Tengu.Business.API.DTO;
using Tengu.Business.Commons.Models;
using Tengu.Dialogs;

namespace Tengu.ViewModels
{
    public class SearchControlViewModel : ViewModelBase
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.MainLogger);

        private AvaloniaList<SearchAnimeModel> animeList = new();
        private string title = string.Empty;
        private Hosts seletctedHost = Hosts.AnimeSaturn;
        private Statuses selectedStatus = Statuses.None;
        private bool loading = false;

        #region Properties
        public ICommand SearchCommand { get; private set; }
        public ICommand OpenAnimeCardCommand { get; private set; }
        public List<Statuses> StatusesList { get; set; }
        public List<GenresModel> GenresList { get; private set; }
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }
        public AvaloniaList<SearchAnimeModel> AnimeList
        {
            get => animeList;
            set => this.RaiseAndSetIfChanged(ref animeList, value);
        }
        public Hosts SelectedHost
        {
            get => seletctedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref seletctedHost, value);
                ProgramConfig.Hosts.Search = SelectedHost;
            }
        }
        public Statuses SelectedStatus 
        { 
            get => selectedStatus;
            set => this.RaiseAndSetIfChanged(ref selectedStatus, value);
        }
        #endregion

        public SearchControlViewModel()
        {
            SearchCommand = ReactiveCommand.Create(ExecuteSearch);
            OpenAnimeCardCommand = ReactiveCommand.Create<SearchAnimeModel>(ShowAnimeCard);

            HostsList.Add(Hosts.None);

            StatusesList = EnumExtension.ToList<Statuses>();

            GenresList = new();
            EnumExtension.ToList<Genres>().ForEach(x =>
            {
                if (!x.Equals(Genres.None))
                    GenresList.Add(new(x));
            });

            SelectedHost = ProgramConfig.Hosts.Search;
        }

        private async void ExecuteSearch()
        {
            Loading = true;
            AnimeList.Clear();

            try
            {
                TenguApi.CurrentHosts = SelectedHost.Equals(Hosts.None) ?
                    new Hosts[] { Hosts.AnimeUnity, Hosts.AnimeUnity } :
                       new Hosts[] { SelectedHost };

                Genres[] genres = GenresList.Where(x => x.IsChecked).Select(x => x.Genre).ToArray();

                SearchFilter filter = new()
                {
                    Status = SelectedStatus,
                    Genres = genres
                };

                log.Info("Search >> Host: {host} | Status: {status} | Genres: {genres}", SelectedHost, SelectedStatus, string.Join(',', genres));

                TenguResult<AnimeModel[]> res = await TenguApi.SearchAnimeAsync(Title, filter);

                foreach (TenguResultInfo infoRes in res.Infos)
                {
                    if (!infoRes.Success)
                    {
                        InfoBar.AddMessage($"Search Error ({infoRes.Host})",
                                   infoRes.Exception.Message,
                                   InfoBarSeverity.Error);

                        log.Error(infoRes.Exception, "ExecuteSearch >> SearchAnimeAsync | Host: {host}", infoRes.Host);
                    }
                }

                foreach (AnimeModel anime in res.Data)
                {
                    AnimeList.Add(new(anime));
                }
            }
            catch (Exception ex)
            {
                InfoBar.AddMessage("Search Error",
                                   ex.Message,
                                   InfoBarSeverity.Error);

                log.Error(ex, "ExecuteSearch >> SearchAnimeAsync");
            }
            finally
            {
                Loading = false;
            }
        }

        public async void ShowAnimeCard(SearchAnimeModel searchAnime)
        {
            if (null != searchAnime)
            {
                TenguResult<AnimeModel[]> res = await TenguApi.SearchAnimeAsync(searchAnime.Anime.Title, 1);

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

                if (!res.Data.Any())
                {
                    InfoBar.AddMessage($"Show AnimeCard Error ({searchAnime.Anime.Host})",
                                   $"Anime Card ({searchAnime.Anime.Title}) Not found",
                                   InfoBarSeverity.Error);

                    log.Error("ShowAnimeCard >> SearchAnimeAsync | Host: {host} - Anime not found", searchAnime.Anime.Host);
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
