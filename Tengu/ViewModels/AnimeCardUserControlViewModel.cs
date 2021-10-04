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
using static Tengu.Utilities.PrismEvents;
using Logger = NLog.Logger;

namespace Tengu.ViewModels
{
    public class AnimeCardUserControlViewModel : BindablePropertyBase, INavigationAware
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private NotificationManager notificationManager;

        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private string anime_card_url;
        private bool is_loading;
        private BackgroundWorker refresh_worker;

        private Card_Data anime_card;
        private Card_Data temp_card;

        #region Properties
        public ICommand CommandGoBack { get; private set; }
        public ICommand CommandShowCard { get; private set; }
        public ICommand CommandDownloadEpisode { get; private set; }
        public Card_Data AnimeCard
        {
            get { return anime_card; }
            set
            {
                anime_card = value;
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

        public AnimeCardUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            anime_card = new Card_Data();
            temp_card = new Card_Data();

            notificationManager = new NotificationManager();

            CommandGoBack = new SimpleRelayCommand(GoBack);
            CommandShowCard = new RelayCommand<RelatedData>(ShowAnimeCard);
            CommandDownloadEpisode = new RelayCommand<Anime>(DownloadEpisode);
        }
        public AnimeCardUserControlViewModel() { }

        private bool CanGoBack()
        {
            return _regionManager.Regions["ControlsRegion"].NavigationService.Journal.CanGoBack;
        }

        private void GoBack()
        {
            if (CanGoBack())
            {
                _regionManager.Regions["ControlsRegion"].NavigationService.Journal.GoBack();
            }
        }

        private void DownloadEpisode(Anime ep)
        {
            AnimeData anime = new(_eventAggregator)
            {
                Title = ep.Title,
                Episode = ep.Episode,
                ImageCover = ep.ImageCover,
                ImagePoster = ep.ImagePoster,
                LinkCard = ep.LinkCard,
                LinkVideo = ep.LinkVideo,
                Description = ep.Description
            };

            _eventAggregator.GetEvent<AddAnimeToDownloadListEvent>().Publish(anime);
        }

        private void ShowAnimeCard(RelatedData related)
        {
            // Define navigation parameters
            NavigationParameters navigationParams = new();
            navigationParams.Add("AnimeCardUrl", related.Link);

            _regionManager.RequestNavigate("ControlsRegion", "AnimeCardUserControl", navigationParams);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Do nothing .. 
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // Get URL from Navigation parameters
            anime_card_url = (string)navigationContext.Parameters["AnimeCardUrl"];

            // Refresh Datas
            RefreshData();
        }

        public void RefreshData()
        {
            if (!IsLoading)
            {
                if (refresh_worker == null)
                {
                    refresh_worker = new BackgroundWorker();
                }

                log.Info("Refreshing card..");

                refresh_worker.DoWork += Refresh_worker_DoWork; ;
                refresh_worker.RunWorkerCompleted += Refresh_worker_RunWorkerCompleted;

                refresh_worker.RunWorkerAsync();
            }
        }

        #region BackgroundWorker
        private void Refresh_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                log.Info("Card Refresh Completed");

                AnimeCard.Title = temp_card.Title;
                AnimeCard.Plot = temp_card.Plot;
                AnimeCard.ImageLink = temp_card.ImageLink;

                AnimeCard.Tags.CustomAddRange(temp_card.Tags);
                AnimeCard.UsefulLinks.CustomAddRange(temp_card.UsefulLinks);
                AnimeCard.Related.CustomAddRange(temp_card.Related);
                AnimeCard.Attributes.CustomAddRange(temp_card.Attributes);
                AnimeCard.Episodes.CustomAddRange(temp_card.Episodes);

                //DebugCard(temp_card);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Anime Card Error!", ex.Message);
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
                IsLoading = true;

                // Scrapper: Refresh Anime Card
                temp_card = ScrapperService.Instance.RefreshAnimeCard(anime_card_url);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Anime Card Error!", ex.Message);
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
    }
}
