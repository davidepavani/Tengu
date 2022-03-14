using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Collections;
using NLog;
using ReactiveUI;
using Splat;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Enums;
using Tengu.Logging;

namespace Tengu.ViewModels
{
    public class UpcomingPageViewModel : ReactiveObject
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.UpcomingLoggerName);

        private readonly ITenguApi tenguApi;
        
        private AvaloniaList<KitsuAnimeModel> animeList = new();
        
        private KitsuAction currentAction = KitsuAction.None;
        private int offset = 0;

        private string animeTitle = string.Empty;
        private bool loadingAnimes = false;
        private int currentPage = 0;

        private bool canPrev = false;
        private bool canNext = false;

        #region Properties
        public ICommand SearchCommand { get; set; }
        public ICommand UpcomingCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public ICommand PrevCommand { get; set; }
        public AvaloniaList<KitsuAnimeModel> AnimeList
        {
            get => animeList;
            set => this.RaiseAndSetIfChanged(ref animeList, value);
        }
        public string AnimeTitle
        {
            get => animeTitle;
            set => this.RaiseAndSetIfChanged(ref animeTitle, value);
        }
        public bool CanPrev
        {
            get => canPrev;
            set => this.RaiseAndSetIfChanged(ref canPrev, value);
        }
        public bool CanNext
        {
            get => canNext;
            set => this.RaiseAndSetIfChanged(ref canNext, value);
        }
        public bool LoadingAnimes
        {
            get => loadingAnimes;
            set => this.RaiseAndSetIfChanged(ref loadingAnimes, value);
        }
        public int CurrentPage
        {
            get => currentPage;
            set => this.RaiseAndSetIfChanged(ref currentPage, value);
        }
        #endregion

        public UpcomingPageViewModel()
        {
            tenguApi = Locator.Current.GetService<ITenguApi>();

            SearchCommand = ReactiveCommand.Create(SearchAnimes);
            UpcomingCommand = ReactiveCommand.Create(UpcomingAnimes);
            PrevCommand = ReactiveCommand.Create(PrevPage);
            NextCommand = ReactiveCommand.Create(NextPage);
        }

        public void PrevPage()
        {
            if (CanPrev)
            {
                CurrentPage -= 1;
                offset -= 10;

                log.Trace($"Prev page >> {CurrentPage}");

                _ = LoadAnimes(currentAction);
            }
        }
        public void NextPage()
        {
            if (CanNext)
            {
                CurrentPage += 1;
                offset += 10;

                log.Trace($"Next page >> {CurrentPage}");

                _ = LoadAnimes(currentAction);
            }
        }

        private void SearchAnimes()
        {
            Clear();

            log.Info("Searching Animes..");
            log.Info($"Anime Title: {AnimeTitle}");

            currentAction = KitsuAction.Search;
            _ = LoadAnimes(KitsuAction.Search);
        }
        private void UpcomingAnimes()
        {
            Clear();

            log.Info("Getting Upcoming..");
            
            currentAction = KitsuAction.Upcoming;
            _ = LoadAnimes(KitsuAction.Upcoming);
        }

        private async Task LoadAnimes(KitsuAction kitsu)
        {
            LoadingAnimes = true;
            AnimeList.Clear();

            foreach (KitsuAnimeModel anime in await ExecuteSearch(kitsu))
            {
                AnimeList.Add(anime);
            }

            log.Info($"Animes found: {AnimeList.Count}");

            CanPrev = !CurrentPage.Equals(0) && AnimeList.Count > 0;
            CanNext = AnimeList.Count > 0 && AnimeList.Count == 10;

            LoadingAnimes = false;

            log.Info($"{kitsu} ended!");
        }

        private async Task<KitsuAnimeModel[]> ExecuteSearch(KitsuAction kitsu)
        {
            try
            {
                log.Info($"{kitsu} Offset: {offset}");

                return kitsu switch
                {
                    KitsuAction.Search => await tenguApi.KitsuSearchAnimeAsync(AnimeTitle, offset, offset + 10),
                    _ => await tenguApi.KitsuUpcomingAnimeAsync(offset, offset + 10),
                };
            }
            catch(Exception ex)
            {
                // logging
                log.Error(ex, $"{kitsu} Exception");
                return Array.Empty<KitsuAnimeModel>(); 
            }
        }

        private void Clear()
        {
            CurrentPage = 0;
            offset = 0;
        }
    }
}
