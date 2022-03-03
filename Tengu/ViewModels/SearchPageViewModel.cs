using Avalonia.Collections;
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
using Tengu.Extensions;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class SearchPageViewModel : ReactiveObject
    {
        private readonly ITenguApi tenguApi;

        private Hosts selectedHost = Hosts.None;
        private Statuses selectedStatus = Statuses.None;

        private string animeTitle = string.Empty;
        private bool searching = false;
        private AvaloniaList<AnimeModel> animeList = new();
        private AnimeModel[] animes = Array.Empty<AnimeModel>();
        private bool paginationVisible = false;
        private int currentPage = 0;
        private int pageCount = 0;

        #region Properties
        public ICommand PageChangedCommand { get; set; }
        public ICommand SearchCommand { get; private set; }
        public List<Hosts> HostList { get; private set; }
        public List<GenresModel> GenresList { get; private set; }
        public List<Statuses> StatusesList { get; private set; }
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                this.RaiseAndSetIfChanged(ref currentPage, value);
                UpdatePaginationItems(value);
            }
        }
        public int PageCount
        {
            get => pageCount;
            set => this.RaiseAndSetIfChanged(ref pageCount, value);
        }
        public bool PaginationVisible
        {
            get => paginationVisible;
            set => this.RaiseAndSetIfChanged(ref paginationVisible, value);
        }
        public bool Searching
        {
            get => searching;
            set => this.RaiseAndSetIfChanged(ref searching, value);
        }
        public AvaloniaList<AnimeModel> AnimeList
        {
            get => animeList;
            set => this.RaiseAndSetIfChanged(ref animeList, value);
        }
        public string AnimeTitle
        {
            get => animeTitle;
            set => this.RaiseAndSetIfChanged(ref animeTitle, value);
        }
        public Hosts SelectedHost
        {
            get => selectedHost;
            set => this.RaiseAndSetIfChanged(ref selectedHost, value);
        }
        public Statuses SelectedStatus
        {
            get => selectedStatus;
            set => this.RaiseAndSetIfChanged(ref selectedStatus, value);
        }
        #endregion

        public SearchPageViewModel()
        {
            tenguApi = Locator.Current.GetService<ITenguApi>();
            GenresList = new();

            Task.Run(() =>
            {
                HostList = EnumExtension.ToList<Hosts>();
                StatusesList = EnumExtension.ToList<Statuses>();

                EnumExtension.ToList<Genres>().ForEach(x =>
                {
                    if (!x.Equals(Genres.None))
                        GenresList.Add(new(x));
                });
            });

            SearchCommand = ReactiveCommand.Create(SearchAnimes);
        }

        private async Task SearchAnimes()
        {
            Searching = true;

            animes = Array.Empty<AnimeModel>();

            try
            {
                tenguApi.CurrentHosts = SelectedHost.Equals(Hosts.None) ?
                    new Hosts[] { Hosts.AnimeUnity, Hosts.AnimeUnity } :
                       new Hosts[] { SelectedHost };

                SearchFilter filter = new()
                {
                    Status = SelectedStatus,
                    Genres = GenresList.Where(x => x.IsChecked).Select(x => x.Genre).ToArray()
                };

                animes = await tenguApi.SearchAnimeAsync(AnimeTitle, filter);

                PageCount = animes.Length / 8;
                PaginationVisible = PageCount > 1;
                CurrentPage = 0;
            }
            catch(Exception ex)
            {
                // logging
            }
            finally
            {
                Searching = false;
            }
        }

        private void UpdatePaginationItems(int val)
        {
            AnimeList.Clear();

            animes.Skip(val * 8)
                  .Take(8)
                  .ToList()
                  .ForEach(x => AnimeList.Add(x));
        }
    }
}
