using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
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
using Tengu.Extensions;
using Tengu.Models;
using Tengu.WebScrapper;
using Tengu.WebScrapper.Extensions;
using Tengu.WebScrapper.Helpers;
using static Tengu.Utilities.PrismEvents;
using Logger = NLog.Logger;

namespace Tengu.ViewModels
{
    public class LatestEpisodesUserControlViewModel : BindablePropertyBase
    {
        #region Declarations
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private OptimizedObservableCollection<AnimeData> anime_list;
        private NotificationManager notificationManager;
        private List<Anime> temp_list;

        private BackgroundWorker refresh_worker;
        private BackgroundWorker btns_worker;

        private int current_page;
        private bool is_loading;
        #endregion

        #region Properties
        public ICommand CommandShowCard { get; private set; }
        public ICommand CommandNext { get; private set; }
        public ICommand CommandPrevious { get; private set; }
        public ICommand CommandEnqueue { get; private set; }
        public OptimizedObservableCollection<AnimeData> AnimeList
        {
            get { return anime_list; }
            set
            {
                anime_list = value;
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
        public int CurrentPage
        {
            get { return current_page; }
            set
            {
                current_page = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public LatestEpisodesUserControlViewModel() { }
        public LatestEpisodesUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            AnimeList = new OptimizedObservableCollection<AnimeData>();

            CurrentPage = 1;
            IsLoading = false;

            CommandShowCard = new RelayCommand<AnimeData>(ShowCard);
            CommandEnqueue = new RelayCommand<AnimeData>(EnqueueAnime);
            CommandNext = new SimpleRelayCommand(new Action(NextPage));
            CommandPrevious = new SimpleRelayCommand(new Action(PreviousPage));

            notificationManager = new NotificationManager();

            RefreshAnimeList();
        }
        #endregion

        #region Public Methods
        public void RefreshAnimeList()
        {
            if (!IsLoading)
            {
                if (refresh_worker == null)
                {
                    refresh_worker = new BackgroundWorker();
                }

                refresh_worker.DoWork += Refresh_worker_DoWork;
                refresh_worker.RunWorkerCompleted += Refresh_worker_RunWorkerCompleted;

                AnimeList.Clear();

                refresh_worker.RunWorkerAsync();
            }
        }
        #endregion

        #region Private Methods
        private void ShowCard(AnimeData anime)
        {
            // Define navigation parameters
            NavigationParameters navigationParams = new();
            navigationParams.Add("AnimeCardUrl", AnimeDataHelper.GetAnimeCardFromVideoUrl(anime.LinkVideo));
            
            _regionManager.RequestNavigate("ControlsRegion", "AnimeCardUserControl", navigationParams);
        }

        private void EnqueueAnime(AnimeData anime)
        {
            _eventAggregator.GetEvent<AddAnimeToDownloadListEvent>().Publish(anime);
        }

        private void PreviousPage()
        {
            if (!IsLoading && CurrentPage > 1)
            {
                if (btns_worker == null)
                {
                    btns_worker = new BackgroundWorker();
                }

                btns_worker.DoWork += Btns_worker_DoWork;
                btns_worker.RunWorkerCompleted += Btns_worker_RunWorkerCompleted;

                AnimeList.Clear();

                btns_worker.RunWorkerAsync(false);
            }
        }

        private void NextPage()
        {
            if (!IsLoading)
            {
                if (btns_worker == null)
                {
                    btns_worker = new BackgroundWorker();
                }

                btns_worker.DoWork += Btns_worker_DoWork;
                btns_worker.RunWorkerCompleted += Btns_worker_RunWorkerCompleted;

                AnimeList.Clear();

                btns_worker.RunWorkerAsync(true);
            }
        }
        #endregion

        #region Refresh Worker
        private void Refresh_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Refrsh List
                foreach(Anime anime in temp_list)
                {
                    AnimeList.Add(new AnimeData(_eventAggregator)
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

                log.Info("Refresh Completed..");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Refresh Latest Episodes Error!", ex.Message);
            }
            finally
            {
                IsLoading = false;

                refresh_worker.DoWork -= Refresh_worker_DoWork;
                refresh_worker.RunWorkerCompleted -= Refresh_worker_RunWorkerCompleted;
                refresh_worker.Dispose();
            }
        }

        private void Refresh_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                log.Info("Refreshing Latest Episodes..");
                IsLoading = true;

                temp_list = ScrapperService.Instance.RefreshLatestEpisodes();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Refresh Latest Episodes Error!", ex.Message);
                temp_list = new List<Anime>();
            }
        }
        #endregion

        #region Buttons Worker
        private void Btns_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((bool)e.Result)
                {
                    IsLoading = false;
                    RefreshAnimeList();
                }
                else
                {
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorNotification("Buttons Click Error!", ex.Message);
            }
            finally
            {
                btns_worker.DoWork -= Btns_worker_DoWork;
                btns_worker.RunWorkerCompleted -= Btns_worker_RunWorkerCompleted;
                btns_worker.Dispose();
            }
        }

        private void Btns_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool res = false;

            try
            {
                bool arg = (bool)e.Argument;

                IsLoading = true;

                if (arg)
                {
                    if (ScrapperService.Instance.NextPage())
                    {
                        CurrentPage++;
                        res = true;
                    }
                }
                else
                {
                    if (ScrapperService.Instance.PrevPage())
                    {
                        CurrentPage--;
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorNotification("Buttons Click Error!", ex.Message);
                res = false;
            }

            e.Result = res;
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
    }
}
