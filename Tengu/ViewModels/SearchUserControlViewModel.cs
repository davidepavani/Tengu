using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using HandyControl.Tools.Extension;
using NLog;
using Notification.Wpf;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Models;
using Tengu.WebScrapper;
using Tengu.WebScrapper.Helpers;
using static Tengu.Utilities.PrismEvents;
using Logger = NLog.Logger;

namespace Tengu.ViewModels
{
    public class SearchUserControlViewModel : BindablePropertyBase, INavigationAware
    {
        #region Declarations
        private const int MAX_DATA_PER_PAGE = 10;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private NotificationManager notificationManager;
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        private OptimizedObservableCollection<AnimeData> search_list;
        private List<Anime> temp_list;
        private BackgroundWorker search_worker;
        
        private bool is_loading;
        private string search_text;
        private int max_page_count;

        #endregion

        #region Properties
        public ICommand CommandExecSearch { get; private set; }
        public ICommand CommandShowAnime { get; private set; }
        public int MaxPageCount
        {
            get { return max_page_count; }
            set
            {
                max_page_count = value;
                RaisePropertyChanged();
            }
        }
        public OptimizedObservableCollection<AnimeData> SearchList
        {
            get { return search_list; }
            set
            {
                search_list = value;
                RaisePropertyChanged();
            }
        }
        public bool IsLoading
        {
            get { return is_loading; }
            set
            {
                is_loading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public SearchUserControlViewModel() { }
        public SearchUserControlViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : base()
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            SearchList = new OptimizedObservableCollection<AnimeData>();

            notificationManager = new NotificationManager();

            IsLoading = false;
            MaxPageCount = 1;

            CommandExecSearch = new RelayCommand<string>(ExecuteSearch);
            CommandShowAnime = new RelayCommand<AnimeData>(ShowAnimeCard);

            _eventAggregator.GetEvent<ExecuteSearchEvent>().Subscribe(ExecuteSearch);
        }
        #endregion

        private void ShowAnimeCard(AnimeData anime)
        {
            // Define navigation parameters
            NavigationParameters navigationParams = new();
            navigationParams.Add("AnimeCardUrl", anime.LinkCard);

            _regionManager.RequestNavigate("ControlsRegion", "AnimeCardUserControl", navigationParams);
        }

        private void ExecuteSearch(string search_pattern)
        {
            search_text = search_pattern;

            if (!IsLoading)
            {
                if (string.IsNullOrEmpty(search_text))
                {
                    SearchList.Clear();
                    return; // quit
                }

                log.Info("Search pattern: " + search_pattern);

                if (search_worker == null)
                {
                    search_worker = new BackgroundWorker();
                }

                search_worker.DoWork += Search_worker_DoWork;
                search_worker.RunWorkerCompleted += Search_worker_RunWorkerCompleted;

                search_worker.RunWorkerAsync();
            }
        }

        public void UpdatePagination(int info)
        {
            SearchList.Clear();

            foreach (Anime anime in temp_list.Skip((info - 1) * MAX_DATA_PER_PAGE)
                                             .Take(MAX_DATA_PER_PAGE).ToList())
            {
                SearchList.Add(new AnimeData(_eventAggregator)
                {
                    Title = anime.Title,
                    Episode = anime.Episode,
                    ImageCover = anime.ImageCover,
                    ImagePoster = anime.ImagePoster,
                    LinkCard = anime.LinkCard,
                    LinkVideo = anime.LinkVideo,
                    Description = anime.Description
                });
            }

            log.Info("Pagination updated!");
        }

        #region Refresh Worker
        private void Search_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                SearchList.Clear();

                foreach (Anime anime in temp_list.Take(MAX_DATA_PER_PAGE).ToList())
                {
                    SearchList.Add(new AnimeData(_eventAggregator)
                    {
                        Title = anime.Title,
                        Episode = anime.Episode,
                        ImageCover = anime.ImageCover,
                        ImagePoster = anime.ImagePoster,
                        LinkCard = anime.LinkCard,
                        LinkVideo = anime.LinkVideo,
                        Description = anime.Description
                    });
                }

                log.Info("Search Completed..");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Search Error!", ex.Message);
            }
            finally
            {
                IsLoading = false;

                search_worker.DoWork -= Search_worker_DoWork; ;
                search_worker.RunWorkerCompleted -= Search_worker_RunWorkerCompleted;
                search_worker.Dispose();
            }
        }

        private void Search_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                log.Info("Searching..");

                // Scrapper: Refresh Animes
                temp_list = ScrapperService.Instance.RefreshSearch(search_text);

                if (temp_list.Count >= MAX_DATA_PER_PAGE)
                {
                    MaxPageCount = temp_list.Count / MAX_DATA_PER_PAGE;

                    if(MaxPageCount == 1 &&
                       temp_list.Count % MAX_DATA_PER_PAGE != 0)
                    {
                        MaxPageCount++;
                    }
                }
                else
                {
                    MaxPageCount = 1;
                }

                log.Info("Pages: " + MaxPageCount);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Search Error!", ex.Message);
            }
        }
        #endregion

        public void ShowErrorNotification(string title, string message)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                notificationManager.Show(new NotificationContent
                {
                    Title = title,
                    Message = message,
                    Type = NotificationType.Error
                });
            });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Do Nothing .. 
        }
    }
}
