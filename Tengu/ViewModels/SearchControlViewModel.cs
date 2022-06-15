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

namespace Tengu.ViewModels
{
    public class SearchControlViewModel : ViewModelBase
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.MainLogger);

        private AvaloniaList<AnimeModel> animeList = new();
        private string title = string.Empty;
        private Hosts seletctedHost = Hosts.AnimeSaturn;
        private Statuses selectedStatus = Statuses.None;
        private bool loading = false;

        #region Properties
        public ICommand SearchCommand { get; private set; }
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
        public AvaloniaList<AnimeModel> AnimeList
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

                foreach(AnimeModel anime in await TenguApi.SearchAnimeAsync(Title, filter))
                {
                    AnimeList.Add(anime);
                }
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
